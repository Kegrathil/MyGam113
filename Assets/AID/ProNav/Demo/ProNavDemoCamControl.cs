using UnityEngine;
using System.Collections;

public class ProNavDemoCamControl : MonoBehaviour {

    public GameObject target, missile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 dir = target.transform.position - missile.transform.position;

        transform.position = missile.transform.position + Vector3.up - dir.normalized*2f;

        transform.LookAt(target.transform.position);
	}
}
