using UnityEngine;
using System.Collections;

//based on http://brettbeauregard.com/blog/tag/beginners-pid/

namespace AID{
[System.Serializable]
public class PIDVector3
{
	public float propScale = 0.1f; 
	public float integralScale = 0.1f;	//mul'ed by expected interval
	public float derivativeScale = 0.1f;	//div'ed by expected interval
	public Vector3 intSum;
	public Vector3 lastTarget;
	public float clampValue = 9999999999;
	
	public Vector3 Compute(Vector3 cur, Vector3 target, float dt)
	{
		Vector3 error = target - cur;
		Vector3 propData = propScale * error;
		
		
		Vector3 intD = error * dt * integralScale;
		if(intD.sqrMagnitude > clampValue*clampValue)	intD = intD.normalized * clampValue;
		
		intSum += intD;
		Vector3 intData = intSum;
		
		
		Vector3 derData = derivativeScale * (target - lastTarget) * -1.0f;
		
		
		lastTarget = target;
		Vector3 output = propData + intData + derData;
		if(output.sqrMagnitude > clampValue*clampValue)	output = output.normalized * clampValue;
		return output;
	}

};
}
