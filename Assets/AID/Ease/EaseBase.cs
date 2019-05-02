using UnityEngine;
using System.Collections;
using System;

namespace AID
{
    [System.Serializable]
    public abstract class EaseBase : System.Object
    {
        #region public data
        public float startPoint = 0;
        public float endPoint = 1;
        public float curPoint = 0;
        public EaseMode mode = EaseMode.In;

        //currently everything but loop and pingpong is clamp
        public WrapMode wrapMode;
        #endregion

        #region public functions
        public EaseBase() { }

        public EaseBase(float start, float end)
        {
            startPoint = start;
            endPoint = end;
            curPoint = startPoint;
        }

        public float GetLength()
        {
            return endPoint - startPoint;
        }

        public bool IsAtEnd()
        {
            switch (wrapMode)
            {
                case WrapMode.Clamp:
                    return curPoint >= endPoint;
                //break;
                case WrapMode.Loop:
                    return false;
                //break;
                case WrapMode.PingPong:
                    return false;
                //break;
                case WrapMode.Default:
                    return curPoint >= endPoint;
                //break;
                case WrapMode.ClampForever:
                    return curPoint >= endPoint;
                //break;
                default:
                    return false;
                    //break;
            }
        }

        public void Restart()
        {
            curPoint = startPoint;
        }

        //using this you can pass in the change from last position and this returns the new percentage between start and end.
        public float IncrementValue(float delta)
        {
            curPoint += delta;

            if (IsContinuous())
            {
                //do not let it grow more than 2 times its length for float accuracy 
                if (curPoint > startPoint + GetLength() * 2f)
                    curPoint = ((curPoint - startPoint) % (GetLength() * 2f)) + startPoint;
            }

            //data for this frame, allows pingpong to work
            float framePoint = 0;
            switch (wrapMode)
            {
                case WrapMode.Clamp:
                    framePoint = Mathf.Clamp(curPoint, startPoint, endPoint);
                    break;
                case WrapMode.ClampForever:
                    framePoint = Mathf.Clamp(curPoint, startPoint, endPoint);
                    break;
                case WrapMode.Default:
                    framePoint = Mathf.Clamp(curPoint, startPoint, endPoint);
                    break;
                case WrapMode.Loop:
                    framePoint = ((curPoint - startPoint) % GetLength()) + startPoint;
                    break;
                case WrapMode.PingPong:
                    framePoint = Mathf.PingPong(curPoint - startPoint, GetLength()) + startPoint;
                    break;
            }

            return CalculateValue(framePoint);
        }

        private bool IsContinuous()
        {
            return wrapMode == WrapMode.Default || wrapMode == WrapMode.Loop || wrapMode == WrapMode.PingPong;
        }


        /*
         * Calculate via an input value relative to the specified range
         * 
         * See CalculatePercentage
         */
        public float CalculateValue(float v)
        {
            return CalculatePercentage((v - startPoint) / GetLength()) + startPoint;
        }

        /*
         * Returns a percentage for the given linear t as if it is the current mode. Purpose is to 
         * simplify the creation of tweens for different data types by only requiring that they have 
         * a linear interpolation being written in your code.
         */
        public float CalculatePercentage(float p)
        {
            switch (mode)
            {
                case EaseMode.In:
                    p = InternalCalc(p);
                    break;

                case EaseMode.Out:
                    p = 1.0f - InternalCalc(1.0f - p);
                    break;

                case EaseMode.InOut:
                    if (p <= 0.5f)
                    {
                        p *= 2;
                        p = InternalCalc(p) / 2.0f;
                    }
                    else
                    {
                        p -= 0.5f;
                        p *= 2.0f;
                        p = 1.0f - InternalCalc(1.0f - p);
                        p /= 2.0f;
                        p += 0.5f;
                    }
                    break;
            }

            return p;
        }
        #endregion

        public abstract float InternalCalc(float p);
    }

    public enum EaseType
    {
        Default,
        Linear = Default,
        Quadratic,
        Cubic,
        Quartic,
        Quintic,
        Bounce,
        Circle,
        Elastic,
        Exponential,
        Sinusoidal,
        BackUp
    };

    public enum EaseMode
    {
        In, Out, InOut
    };
}