using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    [CreateAssetMenu(menuName = "SmartPool/DefaultHandler")]
    public class SmartPoolHandler : ScriptableObject
    {        
        public int initialSize = 10;
        [Tooltip("Number of new instances created when an instance is requested but none are available.")]
        public int growStep = 10;

        public virtual void Init(SmartPool prefabSmartPool)
        {
            prefabSmartPool.GrowPoolTo(initialSize);
        }

        public virtual void RequestWhenNoneAvail(SmartPool prefabSmartPool)
        {
            prefabSmartPool.GrowPoolBy(growStep);
        }

        public virtual void DestroyGO(SmartPoolObjectInstance obj)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
        }

        public virtual void PreReturnToUser(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
            obj.transform.CopyLocalsFrom(prefabSmartPool.Prefab.transform);
            obj.gameObject.SetActive(true);
        }

        public virtual void PostInstantiate(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
        }

        public virtual void PreReturnToPool(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}