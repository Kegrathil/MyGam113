using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventData : AID.BaseEventData
{
    public override void Reset()
    {
        i = 0;
    }

    public int i;
}


public class DirectEventUserTest : MonoBehaviour {

    
    //private AID.EventManager em;


	// Use this for initialization
	void Start () {
        //em = GetComponent<AID.EventManager>();
        //em.AddListener<SimpleEventData>(Rec);
        AID.GlobalEventManager.AddListener<SimpleEventData>(Rec);

        //duplicate listens not allowed
        AID.GlobalEventManager.AddListener<SimpleEventData>(Rec);


        AID.GlobalEventManager.RemoveListener<SimpleEventData>(Rec);


        //AID.GlobalEventManager.Instance.AddListener<SimpleEventData>(Rec);

    }
	
	// Update is called once per frame
	void Update ()
    {
        var d = AID.GlobalEventManager.GetEventDataObject<SimpleEventData>();
        d.i = Random.Range(0, 10);
        d.TriggerThisEvent();

	}

    void Rec(SimpleEventData e)
    {
        print(e.i);
    }
}
