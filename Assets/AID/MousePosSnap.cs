using UnityEngine;
using System.Collections;

namespace AID{
public class MousePosSnap : MonoBehaviour {

	float z;

	// Use this for initialization
	void Start () {
		z = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		//ortho hack
		Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPos.z = z;
		transform.position = newPos;
	}
}
}