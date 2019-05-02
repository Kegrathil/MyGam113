using UnityEngine;
using System.Collections;

public class InitVel : MonoBehaviour {

    public Vector3 vel;
	public bool randomise = true;

	// Use this for initialization
	void Start () {
		if(randomise)
			vel = Random.rotation * vel;
        
		GetComponent<Rigidbody>().velocity = vel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
