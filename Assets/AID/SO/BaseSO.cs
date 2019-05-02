using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Base class for our ScriptableObjects, ensures we have some common functionality.
    /// Primarily means there is only one thing to fix if unity makes changes
    /// </summary>
    public class BaseSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public virtual void Awake()
        {
        }

        //don't trust this, doco says it gets called but it doesn't seem to in editor.
        // best to use OnValidate
        public virtual void OnDestroy()
        {
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnBeforeSerialize()
        {
            OnValidate();
        }

        public virtual void OnAfterDeserialize()
        {
        }

        public virtual void OnValidate()
        {
        }
    }
}