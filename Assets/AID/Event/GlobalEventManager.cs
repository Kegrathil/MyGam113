using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AID
{
    /*
        A global static wrapper around an AID.EventManager. See EventManager for more info
    */
    public class GlobalEventManager
    {
        private static GlobalEventManager inst;
        private EventManager eventMan = new EventManager();

        
        static protected GlobalEventManager Instance
        {
            get
            {
                if (inst != null)
                    return inst;

                return inst = new GlobalEventManager();
            }
        }

        static public void AddListener<T>(EventManager.EventDelegate<T> del) where T : BaseEventData, new()
        {
            Instance.eventMan.AddListener<T>(del);
        }

        static public void RemoveListener<T>(EventManager.EventDelegate<T> del) where T : BaseEventData, new()
        {
            Instance.eventMan.RemoveListener<T>(del);
        }

        static public void RemoveAll()
        {
            Instance.eventMan.RemoveAll();
        }

        static public void TriggerEvent<T>(BaseEventData e) where T : BaseEventData, new()
        {
            Instance.eventMan.TriggerEvent<T>(e);
        }

        static public T GetEventDataObject<T>() where T : BaseEventData, new()
        {
            if (Instance == null)
                return null;

            return Instance.eventMan.GetEventDataObject<T>();
        }
    }
}