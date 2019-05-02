using UnityEngine;
using System.Collections;


public class RotationEase : MonoBehaviour {
	
	public AID.Ease ease = new AID.Ease();
	
	//target ppints
	public Vector3 start, end;

	void Update () {
		//would normally cache these but people may want to play with them in the inspector while its running
		Quaternion qs = Quaternion.FromToRotation(Vector3.up, start);
		Quaternion qe = Quaternion.FromToRotation(Vector3.up, end);

		//use ease to change our lerp into something else
		transform.rotation = Quaternion.Slerp(qs,qe,ease.IncrementValue(Time.deltaTime));
	}
}
