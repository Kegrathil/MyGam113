using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Stores a float variable in an SO.
    /// </summary>
    [CreateAssetMenu(menuName = "Variable/Float")]
    public class FloatSO : DataTypeSO<float> { }
    
    /// <summary>
    /// An float variable that can be local or backed by an SO. 
    /// Typically used by a monobehaviour to allow configuring an external int.
    /// </summary>
    [System.Serializable]
    public class FloatVar : DataTypeSOVar<float, FloatSO>
    {
        public FloatVar() { }
        public FloatVar(float v) { localVal = v; }
    }
}
