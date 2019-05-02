using UnityEngine;
using System.Collections;
using System;

namespace AID
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class NormalisedCurveAttribute : PropertyAttribute
    {
        public static void ClampAnimCurve(AnimationCurve c)
        {
            var localKeys = c.keys;
            //keep the curve between 0-1 but just mess it up real good
            for (int i = 0; i < localKeys.Length; i++)
            {
                var key = localKeys[i];
                key.time = Mathf.Clamp(key.time, 0, 1);
                if(i == 0)
                {
                    key.time = 0;
                    key.value = Mathf.Clamp(key.value, 0, 1);
                }
                else if (i == localKeys.Length-1)
                {
                    key.time = 1;
                    key.value = Mathf.Clamp(key.value, 0, 1);
                }
                localKeys[i] = key;
            }
            c.keys = localKeys;
        }
    }
}