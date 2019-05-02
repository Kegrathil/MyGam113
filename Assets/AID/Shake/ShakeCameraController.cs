using UnityEngine;
using System.Collections;

namespace AID{
public struct ShakeCameraInfo
{
	public Vector3 mountForce, lookTargetForce;
	
	public ShakeCameraInfo(Vector3 shakeMountBy, Vector3 shakeLookPosBy)
	{
		mountForce = shakeMountBy;
		lookTargetForce = shakeLookPosBy;
	}
}

/*
 * Dual spring camera shake system. As well as teaking individual spring settings you may also want to teak the lookAt 
 * spring very differently as this controls the direction the camera will be looking during the shake, if they are the
 * same settings then it will always look straight.
 */
public class ShakeCameraController : MonoBehaviour {

	public Rigidbody cameraMount, cameraLookTarget;


	public void OnShakeCamera(ShakeCameraInfo info)
	{
		ShakeCamera(info.mountForce, info.lookTargetForce);
	}


	public void ShakeCamera(Vector3 mountForce, Vector3 lookTargetForce)
	{
		if(cameraMount != null) 
		{
			cameraMount.AddRelativeForce(mountForce);
		}
		
		if(cameraLookTarget != null) 
		{
			cameraLookTarget.AddRelativeForce(lookTargetForce);
		}
	}
}
}