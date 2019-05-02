using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Allows us to refer to DataTypeSO<T> or collect multiple with different Ts in
    /// one list.
    /// </summary>
    public class DataTypeSOBase : BaseSO { }

    /// <summary>
    /// A ScriptableObject that holds a variable, allows for creating and distributing core or
    /// common data among many gameobjects and prefabs. E.g. with a float variant you could share
    /// a starting HP amoung a number of enemy types or even the player's current HP.
    /// 
    /// Has an optional event that can be fired when the value is changed.
    /// 
    /// Houses an internal starting value that is what should be configured in the inspector during
    /// construction of the data, the current value is then used at runtime. These steps are 
    /// here so that we don't have values changing constantly in editor causing syncing problems,
    /// minor unwanted changes being prompted in source control and makes it harder to lose
    /// track of a value during testing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTypeSO<T> : DataTypeSOBase
    {
        [SerializeField]
        T startValue;

        [Tooltip("Optional event that will be fired when the Val is set to a new value.")]
        public EventSO OnChangeEvent;


        [Space()]
        [SerializeField]
        T currentValue;

        public T Value
        {
            get
            {
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                    return startValue;
#endif
                return currentValue;
            }
            set
            {
                if (OnChangeEvent != null && !currentValue.Equals(value))
                    OnChangeEvent.Fire();

#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                    startValue = value;
#endif
                currentValue = value;
            }
        }

        public override string ToString()
        {
            if(Value != null)
                return Value.ToString();
            return "null";
        }

#if UNITY_EDITOR
        public override void OnValidate()
        {
            base.OnValidate();
            if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                currentValue = startValue;
        }
#endif

        public override void Awake()
        {
            base.Awake();
            currentValue = startValue;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            currentValue = startValue;
        }
    }
}