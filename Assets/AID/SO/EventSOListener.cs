using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AID
{
    /// <summary>
    /// The default way of attaching an EventSO to other logic via a UnityEvent.
    /// Provides a simply way to linking other behaviour or data changes in your behaviours
    /// from the triggering of a global event.
    /// 
    /// Will add and remove itself from the EventSO based on enable and active.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class EventSOListener : MonoBehaviour, IEventSOListener
    {
        [Tooltip("The global EventSO to be listened to.")]
        public EventSO eventSO;

        [Tooltip("Called when the EventSO is triggered. Allows calling or altering existing " +
            "code without writting custom handlers repeatedly.")]
        public UnityEvent unityEvent; 

        void OnEnable()
        {
            eventSO.Add(this);
        }

        void OnDisable()
        {
            eventSO.Remove(this);
        }

        public void OnEventFired(EventSO origin)
        {
            unityEvent.Invoke();
        }
    }
}