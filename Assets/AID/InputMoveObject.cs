using UnityEngine;
using System.Collections;

public class InputMoveObject : MonoBehaviour {

	[SerializeField]
	private float speed;
	public float Speed {get{return speed;} set{speed = value;}}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0)*Speed*Time.deltaTime;
	}
}
