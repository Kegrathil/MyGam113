using UnityEngine;
using System.Collections;


namespace AID
{
    [System.Serializable]
    public struct IntRange
    {
        public int lower, upper;

        public static IntRange Mix(IntRange a, IntRange b, float mix)
        {
            IntRange retval = new IntRange();

            retval.lower = (int)(a.lower * mix + b.lower * (1 - mix));
            retval.upper = (int)(a.upper * mix + b.upper * (1 - mix));

            return retval;
        }
    }
}
