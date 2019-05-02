using UnityEngine;
using System.Collections;

public class HitTarget : MonoBehaviour {

    public GameObject target;
	public GameObject hitPre, missPre;
    public float speed = 2;
	public AID.ProjectileLaunchVectorSolver sol = new AID.ProjectileLaunchVectorSolver();

	// Use this for initialization
	void Start () {
        Invoke("Run",0.8f);
	}

    void Run()
    {
		if(sol.Solve(target.transform.position,target.GetComponent<Rigidbody>().velocity, 
		             transform.position, speed,
		             Physics.gravity,
		             target.GetComponent<Rigidbody>().useGravity, GetComponent<Rigidbody>().useGravity))
		{
			GetComponent<Rigidbody>().velocity = sol.LaunchVelocity;
		}

		GameObject hit = null;
		if(sol.CanHit)
		{
			hit = (GameObject)Instantiate(hitPre, sol.ImpactPosition, Quaternion.identity);
		}
		else
		{
			hit = (GameObject)Instantiate(missPre, transform.position, Quaternion.identity);
		}
        
		hit.transform.parent = transform.parent;
    }
}
