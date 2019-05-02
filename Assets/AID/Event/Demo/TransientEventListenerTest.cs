using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransientEventListenerTest : MonoBehaviour {

    //public DirectEventUserTest eut;

	// Use this for initialization
	void Start () {
        AID.GlobalEventManager.AddListener<SimpleEventData>(Listen);

        Destroy(this,1);
		
	}

    void OnDisable()
    {
        AID.GlobalEventManager.RemoveListener<SimpleEventData>(Listen);
    }

    void Listen(SimpleEventData e)
    {
        print(e.i);// + gameObject.name);
    }

    void Called()
    {
        print("called via ue");
    }
}
