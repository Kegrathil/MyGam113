using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// An Event within a ScriptableObject, with a parameter. 
    /// This allows creating events in the project, makes it easy to create new events and attach
    /// objects and prefabs to them, something which is difficult or requires scripting otherwise.
    /// 
    /// This is the base class, derive from and specialise, see EventSOParamInt as an example.
    /// </summary>
    public class EventSOParam<T> : BaseSO
    {
        private List<IEventSOParamListener<T>> listeners = new List<IEventSOParamListener<T>>();

        public int ListenerCount
        {
            get
            {
                return listeners.Count;
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IEventSOParamListener<T>> Listeners
        {
            get
            {
                return listeners.AsReadOnly();
            }
        }

        public void Fire(T param)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEventFired(this, param);
            }
        }

        public void Add(IEventSOParamListener<T> eventSOListener)
        {
            if (!listeners.Contains(eventSOListener))
                listeners.Add(eventSOListener);
        }

        public void Remove(IEventSOParamListener<T> eventSOListener)
        {
            listeners.Remove(eventSOListener);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            listeners = new List<IEventSOParamListener<T>>();
        }
    }
}