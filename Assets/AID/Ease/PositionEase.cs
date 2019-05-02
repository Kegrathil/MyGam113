using UnityEngine;
using System.Collections;


public class PositionEase : MonoBehaviour {

	public AID.Ease ease = new AID.Ease();

	//target points
	public Transform start, end;

	void Update () {
		//use ease to change our lerp into something else
		transform.position = start.position + (end.position - start.position) * ease.IncrementValue(Time.deltaTime);
	}
}
