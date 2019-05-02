using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// An Event within a ScriptableObject. 
    /// This allows creating events in the project, makes it easy to create new events and attach
    /// objects and prefabs to them, something which is often difficult otherwise.
    /// 
    /// This Event sends no additional data with the call, only a reference to the event that called it.
    /// </summary>
    [CreateAssetMenu(menuName = "Event/Simple")]
    public class EventSO : BaseSO
    {
        private List<IEventSOListener> listeners = new List<IEventSOListener>();

        public int ListenerCount
        {
            get
            {
                return listeners.Count;
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IEventSOListener> Listeners
        {
            get
            {
                return listeners.AsReadOnly();
            }
        }

        public void Fire()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEventFired(this);
            }
        }

        public void Add(IEventSOListener eventSOListener)
        {
            if(!listeners.Contains(eventSOListener))
                listeners.Add(eventSOListener);
        }

        public void Remove(IEventSOListener eventSOListener)
        {
            listeners.Remove(eventSOListener);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            listeners = new List<IEventSOListener>();
        }
    }
}