using System;
using UnityEngine;

namespace AID
{
    public struct QuadraticResult
    {
        public int numResults;
        public float r1;
        public float r2;

        public void SetAll(float f)
        {
            r1 = f; r2 = f;
        }

        //returns -1 if none
        public float SmallestAboveZero()
        {
            float res = -Mathf.Infinity;
            if (r1 >= 0 && (res > r1 || res == -Mathf.Infinity))
                res = r1;
            if (r2 >= 0 && (res > r2 || res == -Mathf.Infinity))
                res = r2;

            return res;
        }

        public override string ToString()
        {
            return string.Format("[QuadraticResult: r1={0}, r2={1}]", r1.ToString(), r2.ToString());
        }

    };
}