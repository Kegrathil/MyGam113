using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Stores an string variable in an SO.
    /// </summary>
    [CreateAssetMenu(menuName = "Variable/String")]
    public class StringSO : DataTypeSO<string>
    {
        public override void OnValidate()
        {
            base.OnValidate();
            if (Value == null)
                Value = string.Empty;
        }
    }

    /// <summary>
    /// An string variable that can be local or backed by an SO. 
    /// Typically used by a monobehaviour to allow configuring an external int.
    /// </summary>
    [System.Serializable]
    public class StringVar : DataTypeSOVar<string, StringSO>
    {
        public StringVar() { }
        public StringVar(string v) { localVal = v; }
    }
}
