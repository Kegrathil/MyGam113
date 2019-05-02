using UnityEngine;
using System.Collections;

public class GeneratorToLocalPos : MonoBehaviour {

	public GeneratorDriver driver;
	public Vector3 axis;
	public float axisScale = 1, timeScale = 1;
	private Transform trans;

	// Use this for initialization
	void Start () {
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		trans.localPosition = axis*driver.GenerateIncrement(Time.deltaTime * timeScale)*axisScale;
	}
}
