using UnityEngine;
using System.Collections;

namespace AID{
public class ShakeLookAt : MonoBehaviour {

	public Transform target;
	
	void LateUpdate () {
		transform.LookAt(target.position);
	}
}
}