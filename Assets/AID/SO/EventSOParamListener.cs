using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AID
{
    /// <summary>
    /// The default way of attaching an EventSOParam to other logic via a UnityEvent.
    /// Provides a simply way to linking other behaviour or data changes in your behaviours
    /// from the triggering of a global event.
    /// 
    /// Will add and remove itself from the EventSO based on enable and active.
    /// 
    /// Uses T and U as concrete generic params so we can show the elements in the inspector, if we didn't want
    /// the inspector then we wouldn't need them at all. See EventSoParamListenerInt as an example.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public abstract class EventSOParamListener<T, U, S> : MonoBehaviour, IEventSOParamListener<S> 
        where U : UnityEvent<S> 
        where T : EventSOParam<S>
    {
        [Tooltip("The global EventSO to be listened to.")]
        public T eventSO;

        [SerializeField]
        [Tooltip("Called when the EventSOParam is triggered. Allows calling or altering existing " +
            "code without writting custom handlers repeatedly.")]
        public U unityEvent;

        void OnEnable()
        {
            eventSO.Add(this);
        }

        void OnDisable()
        {
            eventSO.Remove(this);
        }

        public void OnEventFired(EventSOParam<S> origin, S param)
        {
            unityEvent.Invoke(param);
        }
    }
}