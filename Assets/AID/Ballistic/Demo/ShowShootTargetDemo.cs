using UnityEngine;
using System.Collections;

public class ShowShootTargetDemo : MonoBehaviour {

	public GameObject demoObj;
	public float delay = 3f;

	void Start()
	{
		StartCoroutine(Loop());
	}

	IEnumerator Loop()
	{
		while(true)
		{
			GameObject go = (GameObject) Instantiate(demoObj);
			yield return new WaitForSeconds(delay);
			Destroy(go);
		}
	}


}
