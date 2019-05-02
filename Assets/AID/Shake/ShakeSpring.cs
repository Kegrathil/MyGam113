using UnityEngine;
using System.Collections;


namespace AID{
	/*
	 * 1 linear spring per axis. Works in local space and will pull itself towards its starting local position,
	 * 	this is done so it can be childed to anything and not care about its world position.
	 * 
	 * Uses rigidbody for forces, velocity and drag. 
	 * 
	 * To fine tune effects, you will want to tweak, limits, springk and rigidbody drag and mass.
	 */
	[RequireComponent(typeof(Rigidbody))]
	public class ShakeSpring : MonoBehaviour {
		
		
		//limits represent the ends of the linear slide the object is on.
		public Vector2 xLimits = new Vector2(-0.5f,0.5f), yLimits = new Vector2(-0.5f,0.5f), zLimits = new Vector2(-0.5f,0.5f);
	
		//linear spring force per axis
		public Vector3 springK = new Vector3(250,250,250);
		
		private Vector3 workingVecLocalPos = Vector3.zero;
		private Vector3 workingVecRBVel = Vector3.zero;
		private Vector3 appliedForce;
		
		public Vector3 naturalPos;
		
		public void ClampLimits(ref Vector3 pos)
		{
			workingVecRBVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
			//if it hits either end we zero the vel on that axis
			if(OutsideRange(pos.x, xLimits.x, xLimits.y))
				workingVecRBVel.x = 0;
			if(OutsideRange(pos.y, yLimits.x, yLimits.y))
				workingVecRBVel.y = 0;
			if(OutsideRange(pos.z, zLimits.x, zLimits.y))
				workingVecRBVel.z = 0;
			
			GetComponent<Rigidbody>().velocity = transform.TransformDirection(workingVecRBVel);
			
			pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
			pos.y = Mathf.Clamp(pos.y, yLimits.x, yLimits.y);
			pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
		}
	
	
	
		// Use this for initialization
		void Start () {
			naturalPos = transform.localPosition;
			ClampLimits( ref naturalPos);
		}
	
		//based on diff from target postion, apply hooke's law
		void FixedUpdate () {
			workingVecLocalPos = transform.localPosition;
			
			ClampLimits( ref workingVecLocalPos);
			
			appliedForce = transform.localPosition - naturalPos;
			
			//calc forces
			appliedForce.Scale(-springK);
			GetComponent<Rigidbody>().AddRelativeForce(appliedForce);
			
			
			transform.localPosition = workingVecLocalPos;
		}
		
		void OnDrawGizmosSelected()
		{
			Vector3 offset = transform.parent != null ? transform.parent.position : Vector3.zero;
			Vector3 local = transform.localPosition;
			
			offset += local;
			Gizmos.color = Color.magenta;
			Gizmos.DrawCube(offset + naturalPos, new Vector3(0.15f,0.15f,0.15f));
				
			Vector3 s = Vector3.zero,e = Vector3.zero;
			s.x = xLimits.x - local.x;
			e.x = xLimits.y - local.x;
			Gizmos.DrawLine(s + offset,e + offset);
			
			s = Vector3.zero;
			e = Vector3.zero;
			s.y = yLimits.x - local.y;
			e.y = yLimits.y - local.y;
			Gizmos.DrawLine(s + offset,e + offset);
			
			s = Vector3.zero;
			e = Vector3.zero;
			s.z = zLimits.x - local.z;
			e.z = zLimits.y - local.z;
			Gizmos.DrawLine(s + offset,e + offset);
			
		}

	//moved from UTIL for portability
	public static bool OutsideRange(float val, float min, float max)
	{
		return val < min || val > max;
	}
}
}