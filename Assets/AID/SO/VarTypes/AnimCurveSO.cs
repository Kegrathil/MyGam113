using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Stores an AnimationCurve variable in an SO.
    /// </summary>
    [CreateAssetMenu(menuName = "Variable/AnimCurve")]
    public class AnimCurveSO : DataTypeSO<AnimationCurve>
    {
        public override void OnValidate()
        {
            base.OnValidate();
            if (Value == null || Value.length < 2)
                Value = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }

    /// <summary>
    /// An AnimationCurve variable that can be local or backed by an SO. 
    /// Typically used by a monobehaviour to allow configuring an external shared value.
    /// </summary>
    [System.Serializable]
    public class AnimCurveVar : DataTypeSOVar<AnimationCurve, AnimCurveSO>
    {
        public AnimCurveVar() { }
        public AnimCurveVar(AnimationCurve v) { localVal = v; }
    }
}

