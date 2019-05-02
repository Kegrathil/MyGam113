using UnityEngine;
using System.Collections;


public class ScaleEase : MonoBehaviour {
	
	public AID.Ease ease = new AID.Ease();

	//target ppints
	public Vector3 start, end;

	void Update () {
		//use ease to change our lerp into something else
		transform.localScale = start + (end - start) * ease.IncrementValue(Time.deltaTime);
	}
}
