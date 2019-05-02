using UnityEngine;
using System.Collections;

public class RandomForceAtInterval : MonoBehaviour {

    public Vector2 randForce;
    public Vector2 randInterval;

	// Use this for initialization
	void Start () {
        Invoke("ForceAtInterval", Random.Range(randInterval.x, randInterval.y));
	}
	
    void ForceAtInterval()
    {
        Vector3 force = Random.onUnitSphere;
        force *= Random.Range(randForce.x, randForce.y);

        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        Invoke("ForceAtInterval", Random.Range(randInterval.x, randInterval.y));
    }
}
