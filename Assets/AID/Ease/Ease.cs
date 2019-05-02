using UnityEngine;
using System.Collections;
using System;

namespace AID
{
    /*
     * Easing equation utility class based on Robert Penner's. This class is the core that is required
     * for any kind of easing to occur. If you can implement the lerp for your specific data type then
     * using this class to update and feed in as the percentage to your lerp turns it into the desired
     * easing function. This reduces the number of operations in some cases and just makes it easier
     * to add easing to new kinds of data.
     * 
     * http://www.robertpenner.com/easing/
     */
    [System.Serializable]
    public class Ease : EaseBase
    {
        public EaseType type = EaseType.Linear;
        
        
        public override float InternalCalc(float p)
        {
            var t = type;
            switch (t)
            {
                case EaseType.Linear:
                    break;

                case EaseType.Quadratic:
                    p = Mathf.Pow(p, 2.0f);
                    break;

                case EaseType.Cubic:
                    p = Mathf.Pow(p, 3.0f);
                    break;

                case EaseType.Quartic:
                    p = Mathf.Pow(p, 4.0f);
                    break;

                case EaseType.Quintic:
                    p = Mathf.Pow(p, 5.0f);
                    break;

                case EaseType.Bounce:

                    if (p == 0)
                        break;

                    //needed to be reversed
                    p = 1.0f - p;

                    if (p < (1.0f / 2.75f))
                    {
                        p = (7.5625f * p * p);
                    }
                    else if (p < 2.0f / 2.75f)
                    {
                        p -= 1.5f / 2.75f;
                        p = 7.5625f * p * p + 0.75f;
                    }
                    else if (p < 2.5f / 2.75)
                    {
                        p -= 2.25f / 2.75f;
                        p = 7.5625f * p * p + 0.9375f;
                    }
                    else {
                        p -= 2.625f / 2.75f;
                        p = 7.5625f * p * p + 0.984375f;
                    }

                    //needed to be reversed
                    p *= -1.0f;
                    p += 1.0f;

                    break;

                case EaseType.Circle:
                    //needed to be flipped
                    p = -Mathf.Sqrt(1.0f - p * p) + 1;
                    break;

                case EaseType.Elastic:
                    if (p == 0 || p == 1) break;

                    float s = 0.3f / 4.0f;

                    //p *= -1.0f;
                    //p += 1.0f;

                    p = -(Mathf.Pow(2, 10 * (p - 1.0f)) *
                                   Mathf.Sin((p - 1 - s) * (2 * Mathf.PI) / 0.3f));
                    break;

                case EaseType.Exponential:
                    if (p != 0)
                    {
                        p = Mathf.Pow(2.0f, 10.0f * (p - 1.0f));
                    }
                    break;

                case EaseType.Sinusoidal:
                    p = -1.0f * Mathf.Cos(p * Mathf.PI / 2) + 1;
                    break;

                case EaseType.BackUp:
                    p = p * p * (2.70158f * p - 1.70158f);
                    break;

                default:
                    break;
            }

            return p;
        }
    }
}