using UnityEngine;
using System.Collections;

//based on http://brettbeauregard.com/blog/tag/beginners-pid/

[System.Serializable]
public class PIDFloat
{
	public float propScale = 0.1f; 
	public float integralScale = 0.1f;	//mul'ed by expected interval
	public float derivativeScale = 0.1f;	//div'ed by expected interval
	public float intSum;
	public float lastTarget;
	public float clampValue = 9999999999;
	
	public float Compute(float cur, float target, float dt)
	{
		float error = target - cur;
		float propData = propScale * error;
		
		
		float intD = error * dt * integralScale;
		intD = Mathf.Clamp(intD, -clampValue, clampValue);
		intSum += intD;
		float intData = intSum;
		
		
		float derData = derivativeScale * (target - lastTarget) * -1.0f;
		
		
		lastTarget = target;
		float output = propData + intData + derData;
		output = Mathf.Clamp(output, -clampValue, clampValue);
		return output;
	}

};

