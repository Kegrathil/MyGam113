using UnityEngine;
using System.Collections;

/*
 * Limits distance between 2 objects
 */

//TODO
// support rigidbodies not just trans
// support modes other than just master slave
// support axis limits
// support local pos not just world
public class Leash : MonoBehaviour {

	public Transform master, slave;
	public float dist;

	private Vector3 dif;
		
	// Update is called once per frame
	void Update () {
		dif = slave.position - master.position;
		if( dif.magnitude > dist)
		{
			//print ("too far, clamping " + slave.name);
			slave.position = master.position + (dif.normalized * dist);
		}
	}
}
