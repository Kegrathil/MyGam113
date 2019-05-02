using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float rotScale = 10;
	public float moveScale = 2;
	public Transform cam, bob, idle;
    public float leanForce, leanOffSetToAngleScale;
    public Rigidbody leanBody;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
		transform.Rotate(0,h * Time.deltaTime * rotScale,0);
		transform.Translate(0,0,v * Time.deltaTime * moveScale);
	
		v = Mathf.Abs(v);
		v = 1-v;
		v*=v;
		v = 1-v;
		
		cam.transform.position = Vector3.Lerp(idle.position, bob.position, v);

        leanBody.AddRelativeForce(h*leanForce,0,0);
        leanBody.transform.localRotation = Quaternion.Euler(0, 0, leanBody.transform.localPosition.x * -leanOffSetToAngleScale);
        cam.localRotation = leanBody.transform.localRotation;
	}
}
