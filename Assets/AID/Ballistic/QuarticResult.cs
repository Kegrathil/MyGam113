using System;
using UnityEngine;

namespace AID
{
    public struct QuarticResult
    {
        public ComplexNumber r1;
        public ComplexNumber r2;
        public ComplexNumber r3;
        public ComplexNumber r4;

        public void SetAll(float f)
        {
            r1.r = f; r2.r = f; r3.r = f; r4.r = f;
            r1.i = 0; r2.i = 0; r3.i = 0; r4.i = 0;
        }

        public int GetNumReals()
        {
            int res = 0;
            if (r1.IsReal) res++;
            if (r2.IsReal) res++;
            if (r3.IsReal) res++;
            if (r4.IsReal) res++;

            return res;
        }

        //returns -Mathf.Infinity if none
        public float SmallestRealAboveZero()
        {
            float res = -Mathf.Infinity;
            if (r1.IsReal && r1.r >= 0 && (res > r1.r || res == -Mathf.Infinity))
                res = r1.r;
            if (r2.IsReal && r2.r >= 0 && (res > r2.r || res == -Mathf.Infinity))
                res = r2.r;
            if (r3.IsReal && r3.r >= 0 && (res > r3.r || res == -Mathf.Infinity))
                res = r3.r;
            if (r4.IsReal && r4.r >= 0 && (res > r4.r || res == -Mathf.Infinity))
                res = r4.r;

            return res;
        }

        public override string ToString()
        {
            return string.Format("[QuarticResult: r1={0}, r2={1}, r3={2}, r4={3}]", r1.ToString(), r2.ToString(), r3.ToString(), r4.ToString());
        }

    };
}