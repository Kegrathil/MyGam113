using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace AID
{
    //originally inspired by http://www.bendangelo.me/unity3d/2014/12/24/unity3d-event-manager.html
    
    public class EventManager
    {
        public delegate void EventDelegate<T>(T e) where T : BaseEventData, new();
        private delegate void EventDelegate(BaseEventData e);
        
        private Dictionary<System.Type, EventManagerChannel> delegates = new Dictionary<System.Type, EventManagerChannel>();
        
        public void AddListener<T>(EventDelegate<T> del) where T : BaseEventData, new()
        {
            var id = GetViaType<T>(true);

            id.Add(del);
        }

        public void RemoveListener<T>(EventDelegate<T> del) where T : BaseEventData, new()
        {
            EventManagerChannel id = GetViaType<T>(false);
            if (id != null)
            {
                id.Remove(del);
            }
            else
            {
                Debug.LogWarning("Event: " + typeof(T) + " has not been registered");
            }
        }

        public void RemoveAll()
        {
            foreach (var item in delegates.Values)
            {
                item.RemoveAll();
            }

            delegates.Clear();
        }
        
        private EventManagerChannel GetViaType<T>(bool createIfNull) where T : BaseEventData, new()
        {
            EventManagerChannel retval = null;
            delegates.TryGetValue(typeof(T), out retval);

            if(createIfNull && retval == null)
            {
                retval = new EventManagerChannel();
                retval.Init<T>();
                delegates[typeof(T)] = retval;
            }

            return retval;
        }

        //You don't currently have to do this as the BaseEventData can be retrieved, init'ed and 
        //  fired from itself
        public void TriggerEvent<T>(BaseEventData e) where T : BaseEventData, new()
        {
            EventManagerChannel id = GetViaType<T>(false);
            if (id != null)
            {
                id.Invoke(e);
            }
            else
            {
                Debug.LogWarning("Event: " + e.GetType() + " has not been registered");
            }
        }

        public T GetEventDataObject<T>() where T : BaseEventData, new()
        {
            var res = GetViaType<T>(true);

            res.EventData.Reset();

            return (T)res.EventData;
        }


        //Helper class for managing 1 type of eventdelegate
        //this should be able to queue so you can fire events from within handlers
        public class EventManagerChannel
        {
            public void Init<T>() where T : BaseEventData, new()
            {
                sharedEventData = new T();
                sharedEventData.owner = this;
            }

            //note that our use of 1 sharedEventData means it needs to be treated as read only and invalid immediately after the event is raised
            private BaseEventData sharedEventData;
            public BaseEventData EventData { get {return sharedEventData;}}
            
            private EventDelegate theDel;
            private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

            public void Add<T>(EventDelegate<T> del) where T : BaseEventData, new()
            {
                EventDelegate internalDel = null;
                delegateLookup.TryGetValue(del, out internalDel);
                if(internalDel != null)
                {
                    Debug.LogWarning("Multiple registrations attmpted in " + typeof(T) + ": " + del.ToString() + 
                    " This is not allowed");
                    return;
                }
                
                // Create a new non-generic delegate which calls our generic one.
                // This is the delegate we actually invoke.
                EventDelegate internalDelegate = (e) => del((T)e);
                delegateLookup[del] = internalDelegate;

                if (theDel == null)
                {
                    theDel = internalDelegate;
                }
                else
                {
                    theDel += internalDelegate;
                }
            }

            public void Invoke(BaseEventData e)
            {
                if (theDel != null)
                    theDel.Invoke(e);
                else
                {
                    Debug.LogWarning(e.GetType() + " event raised but has no listeners");
                }
            }

            public void Remove<T>(EventDelegate<T> del) where T : BaseEventData, new()
            {
                EventDelegate internalDel = null;
                delegateLookup.TryGetValue(del, out internalDel);

                if (internalDel != null)
                {
                    delegateLookup.Remove(del);
                    theDel -= internalDel;
                }
                else
                {
                    Debug.LogWarning("Tried to remove delegate that doesnt exist");
                }
            }

            public void RemoveAll()
            {
                delegateLookup.Clear();
                sharedEventData.owner = null;
                sharedEventData = null;
                theDel = null;
            }
        }
    }
}