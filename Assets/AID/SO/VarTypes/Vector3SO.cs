using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Stores an Vector3 variable in an SO.
    /// </summary>
    [CreateAssetMenu(menuName = "Variable/Vector3")]
    public class Vector3SO : DataTypeSO<Vector3> { }

    /// <summary>
    /// An Vector3 variable that can be local or backed by an SO. 
    /// Typically used by a monobehaviour to allow configuring an external shared value.
    /// </summary>
    [System.Serializable]
    public class Vector3Var : DataTypeSOVar<Vector3, Vector3SO>
    {
        public Vector3Var() { }
        public Vector3Var(Vector3 v) { localVal = v; }
    }
}
