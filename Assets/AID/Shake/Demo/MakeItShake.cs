using UnityEngine;
using System.Collections;

public class MakeItShake : MonoBehaviour {

	public GameObject[] cameras;

	public float small, med, large, SUPER;

	void Update()
	{
		float amt = 0;
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			amt = small;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			amt = med;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			amt = large;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			amt = SUPER;
		}

		if(amt != 0)
		{
			Vector3 m = Random.onUnitSphere * amt;

			Vector3 t = Random.onUnitSphere * amt;

			foreach (GameObject c in cameras)
				c.SendMessage("OnShakeCamera", new AID.ShakeCameraInfo( m,t ));
		}
	}
}
