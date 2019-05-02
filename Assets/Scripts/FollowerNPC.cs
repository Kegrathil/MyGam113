using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerNPC : MonoBehaviour
{
    public enum States
    {
        Follow,
        Idle,
        RandomWalk,
    }

    public States currentState = States.Follow;
    public States previousState = States.Follow;
    public Transform player;
    NavMeshAgent m_Agent;
    NavMeshPath thisPath;
    public Vector3 offset;
    public float inRange;
    //RaycastHit m_HitInfo = new RaycastHit();
    public Vector3 range;
    public Transform startingLoc;
    public int myCurrentIndex = 0;
    bool isWaiting = false;
    public float minDelay;
    public float maxDelay;
    public List<Transform> locations = new List<Transform>();
    public string myName;
    bool battleStarted;
    Collider enemyCollider;
    private Vector3 lastPos;
    bool validPath;



    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        thisPath = new NavMeshPath();
    }

    void Update()
    {
        validPath = NavMesh.CalculatePath(m_Agent.transform.position, player.position, NavMesh.AllAreas, thisPath);


        if (currentState == States.Follow)
        {


            m_Agent.destination = player.position + offset;


        }

        if (currentState == States.RandomWalk)
        {
            StartCoroutine(ChooseNewPos());
        }

        if (m_Agent.hasPath && currentState == States.Follow && m_Agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            ChangePreviousState(currentState);
            ChangeState(States.RandomWalk);
        }

        if (currentState == States.RandomWalk && validPath)
        {

            if (thisPath.status == NavMeshPathStatus.PathComplete)
            {
                ChangePreviousState(currentState);
                ChangeState(States.Follow);
            }
        }

        if (Vector3.Distance(transform.position, player.position) < inRange)
        {
            ChangeState(States.Idle);
        }

        if (Vector3.Distance(transform.position, player.position) > inRange && currentState == States.Idle)
        {
            ChangeState(States.Follow);
        }
    }

    private IEnumerator ChooseNewPos()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (locations == null || locations.Count == 0)
            {
                lastPos = startingLoc.position + new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), Random.Range(-range.z, range.z));
            }
            else
            {
                lastPos = locations[Random.Range(0, locations.Count)].position;
            }

            m_Agent.destination = lastPos;
            isWaiting = false;
        }


    }

    public void ChangeState(States newState)
    {
        currentState = newState;
    }

    public void ChangePreviousState(States newState)
    {
        previousState = newState;
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
    }
}
