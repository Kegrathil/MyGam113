using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Stores an Int variable in an SO.
    /// </summary>
    [CreateAssetMenu(menuName = "Variable/Int")]
    public class IntSO : DataTypeSO<int> { }

    /// <summary>
    /// An Integer variable that can be local or backed by an SO. 
    /// Typically used by a monobehaviour to allow configuring an external int.
    /// </summary>
    [System.Serializable]
    public class IntVar : DataTypeSOVar<int, IntSO>
    {
        public IntVar() { }
        public IntVar(int v) { localVal = v; }
    }
}
