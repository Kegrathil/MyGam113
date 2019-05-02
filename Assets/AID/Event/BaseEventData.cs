using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    public abstract class BaseEventData
    {
        public EventManager.EventManagerChannel owner;

        public abstract void Reset();

        public void TriggerThisEvent()
        {
            if (owner != null)
                owner.Invoke(this);
            else
                Debug.Log("EventData has no owner set, needs to be invoked via EventManager manually.");
        }
    }
}