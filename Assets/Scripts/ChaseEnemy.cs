using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A very simple state based vision detecting enemy demo
/// 
/// Uses the monster love enum based fsm.
/// 
/// If this was a real enemy, it would use awareness of player and multi ray cast every frame regardless of 
/// state and change state when thresholds are met.
/// 
/// In non demo terms this would either have a base class or need ways to communicate to the outside world
/// </summary>
public class ChaseEnemy : MonoBehaviour
{
    const float DEBUG_RAY_TIME_HIT = 0.15f;
    const float DEBUG_RAY_TIME_MISS = 0.5f;
    //this is here to keep the demo small but this kind of audio bark system shouldn't be defined
    //  within the ChaseEnemy Script
    [System.Serializable]
    public class BarkBank
    {
        public States state;
        public AudioClip[] clips; 
    }
    public List<BarkBank> barkBank = new List<BarkBank>();
    public AudioSource audioSource;
    
    public enum States
    {
        Idle,
        Alert,
        Chase, //keep checking for visual
        LastSighting,
        LookingAround,
        ReturnToPost,
        Dead,
    }

    public NavMeshAgent navAgent;
    public Vector3 ourPost, targetPos;


    //ray casts for vision cone
    public float maxDist = 10;
    public float visionConeAngle = 45f;
    public float closeEnough = 1;

    public float lookingAroundSpinSpeed = 1;
    public float lookingAroundTime = 1;
    private float counter = 0;

    private Tim theTim;

    StateMachine<States> fsm;

    void Start()
    {
        theTim = FindObjectOfType<Tim>();

        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Idle);

        ourPost = transform.position;
    }

    public void GoToAlert()
    {
        fsm.ChangeState(States.Alert);
    }

    //a simple vision cone with ray cast for behind a wall check
    public bool CanSeeTim()
    {
        var ourForward = transform.forward;

        var dif = theTim.transform.position - transform.position;

        var distToTim = dif.magnitude;
        var dirToTim = dif.normalized;

        if (distToTim > maxDist)
            return false;
        //if (Vector3.Dot(ourForward, dirToTim) < visionConeAngle) //this is comparing against raw dot product not angle
        //    return;
        var angleBetween = Mathf.Abs(Vector3.Angle(ourForward, dirToTim));
        if (angleBetween > visionConeAngle)
            return false;

        //this should be fancier as we currently can be blocked by a 1 epsilon sphere held along vector
        // same bug as in old hl2 ai
        RaycastHit hit;
        Physics.Raycast(transform.position, dirToTim, out hit);
        if (hit.collider != null)
        {
            if (!hit.collider.gameObject.CompareTag("Tim"))
            {
                Debug.DrawLine(transform.position, hit.point, Color.black, DEBUG_RAY_TIME_MISS);
                return false;
            }
        }

        Debug.DrawLine(transform.position, hit.point, Color.red, DEBUG_RAY_TIME_HIT);
        targetPos = theTim.transform.position;
        targetPos.y = transform.position.y;
        return true;
    }

    IEnumerator Idle_Enter()
    {
        Debug.Log("Just Chillin'");
        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.Idle));
    }

    void Idle_Update()
    {
        Debug.Log("Looking out for Tim.");
        //vision cone for tims

        //ideally this builds awareness rather than snaps
        if (CanSeeTim())
            fsm.ChangeState(States.Alert);
    }

    IEnumerator Alert_Enter()
    {
        Debug.Log("TIM IS HERE!");

        //stop here
        navAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.Alert));

        //after em
        navAgent.SetDestination(targetPos);
        fsm.ChangeState(States.Chase);
    }

    IEnumerator Chase_Enter()
    {
        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.Chase));
    }

    void Chase_Update()
    {
        if (CanSeeTim())
        {
            navAgent.SetDestination(targetPos);
        }
        else
        {
            fsm.ChangeState(States.LastSighting);
        }
    }

    IEnumerator LastSighting_Enter()
    {
        navAgent.SetDestination(transform.position);
        Debug.Log("Lost our Tim, proceedig to last location");
        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.LastSighting));
        navAgent.SetDestination(targetPos);
    }

    void LastSighting_Update()
    {
        if (CanSeeTim())
        {
            fsm.ChangeState(States.Alert);
        }
        

        //if at destination
        if (Vector3.Distance(transform.position, targetPos) < closeEnough)
        {
            fsm.ChangeState(States.LookingAround);
        }
    }

    IEnumerator LookingAround_Enter()
    {
        transform.Rotate(0, lookingAroundSpinSpeed * lookingAroundTime / 2, 0);
        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.LookingAround));
    }

    void LookingAround_Update()
    {
        transform.Rotate(0, lookingAroundSpinSpeed * Time.deltaTime, 0);
        counter += Time.deltaTime;

        if (CanSeeTim())
        {
            fsm.ChangeState(States.Chase);
        }

        if (counter > lookingAroundTime)
        {
            fsm.ChangeState(States.ReturnToPost);
        }
    }

    IEnumerator ReturnToPost_Enter()
    {
        Debug.Log("Sad, no tim.");

        yield return new WaitForSeconds(PlayClipAndTellMeHowLongToWait(States.ReturnToPost));
        navAgent.SetDestination(ourPost);
    }


    void ReturnToPost_Update()
    {
        if (CanSeeTim())
        {
            fsm.ChangeState(States.Chase);
        }

        if (Vector3.Distance(transform.position, ourPost) < closeEnough)
        {
            fsm.ChangeState(States.Idle);
        }
    }



    public float PlayClipAndTellMeHowLongToWait(States state)
    {
        var res = barkBank.Find(x => x.state == state);

        if (res != null)
        {
            var clip = res.clips[Random.Range(0, res.clips.Length)];
            audioSource.PlayOneShot(clip);
            return clip.length / audioSource.pitch;
        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position, transform.forward * maxDist);
        var rot = Quaternion.Euler(0, visionConeAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rot * maxDist);
        var negRot = Quaternion.Euler(0, -visionConeAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, negRot * maxDist);
    }
}
