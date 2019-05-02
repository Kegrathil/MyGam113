using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Base class for all DataTypeSOVar, gives us a way to have a generic property drawer for all.
    /// </summary>
    public abstract class DataTypeSOVarBase { }

    /// <summary>
    /// Generic base type for use in user code, allows for a variable to be either local,
    /// meaning just like any other instance variable, unique per instance. Or backed by a 
    /// DataTypeSO of the same type, allowing data or values to be shared and commonly 
    /// mutated by all instances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U">Concrete class for DataTypeSO<T> required so we can show it in the inspector</typeparam>
    [System.Serializable]
    public class DataTypeSOVar<T, U> : DataTypeSOVarBase
        where U : DataTypeSO<T>
    {
        public DataTypeSOVar(){}
        public DataTypeSOVar(T v){ localVal = v; }

        //value used if the refval is not set
        [SerializeField]
        protected T localVal;

        //ref val to the DataTypeSO, if it is set it is used
        [SerializeField]
        protected U refVal;


        public U SOReferenceValue
        {
            get
            {
                return refVal;
            }
            set
            {
                refVal = value;
            }
        }

        /// <summary>
        /// Property to hide whether local or ref is being used. This is the only way the value should be changed,
        /// not directy on the privaty serialised fields.
        /// </summary>
        public T Value
        {
            get
            {
                if (refVal != null)
                    return refVal.Value;
                else
                    return localVal;
            }
            set
            {
                if (refVal != null)
                    refVal.Value = value;
                else
                    localVal = value;
            }
        }
    }
}