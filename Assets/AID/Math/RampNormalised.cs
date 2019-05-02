using UnityEngine;
using System.Collections;

namespace AID
{
    [System.Serializable]
    public class RampNormalised
    {
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public float remappedMin = 0, remappedMax = 1;

        public void Validate()
        {
            remappedMax = Mathf.Max(remappedMin, remappedMax);
            
            NormalisedCurveAttribute.ClampAnimCurve(curve);
        }

        public float Evaluate(float x)
        {
            return curve.Evaluate(AID.UTIL.Remap(x, remappedMin, remappedMax, 0, 1));
        }
    }
}