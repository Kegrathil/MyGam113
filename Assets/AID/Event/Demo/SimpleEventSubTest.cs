using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventSubTest : MonoBehaviour {

	void Start ()
    {
        AID.GlobalEventManager.AddListener<SimpleEventData>(Rec);
	}
    
    void Rec(SimpleEventData e)
    {
        print(e.i);
    }

    void OnDestroy()
    {
        AID.GlobalEventManager.RemoveListener<SimpleEventData>(Rec);
    }
}
