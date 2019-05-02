using UnityEngine;
using System.Collections;

public class ProNavDemoMissile : MonoBehaviour {

	public float delay = 1f;
    private float proNavK = 4;
    public float maxAccel = 100f;
	public float maxAccelRnd = 20f;
    public float initialRandomVelScale = 0;

    public GameObject target;

    void Start()
    {
		maxAccel += Random.Range(-maxAccelRnd, maxAccelRnd);
        proNavK += Random.Range(-1.0f,1.0f);

        GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * initialRandomVelScale;
    }

    void FixedUpdate()
    {
        Vector3 force = AID.ProNav.CalcAccel(target.transform.position, target.GetComponent<Rigidbody>().velocity,
                                        transform.position, GetComponent<Rigidbody>().velocity,
                                            proNavK);

        if(force.magnitude > maxAccel)
            force = force.normalized * maxAccel;

        GetComponent<Rigidbody>().AddForce(force);
    }
    
    void OnTriggerEnter(Collider col)
    {
    	if(col.CompareTag("Player"))
    	{
    		enabled = false;
    		GetComponent<Rigidbody>().velocity = Vector3.zero;
    		
    		//disable child cam if it exists
    		Camera cam = GetComponentInChildren<Camera>();
    		if(cam)
    			cam.enabled = false;
    	}
    }
}
