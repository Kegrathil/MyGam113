using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    //https://en.wikipedia.org/wiki/Xorshift
    public class XorShift32
    {
        public uint Current { get; set; }
        public uint Seed
        {
            get
            {
                return Current;
            }

            set
            {
                Current = value;
            }
        }

        public uint Next
        {
            get
            {
                Current = XorShift32RandUInt(Current);
                return Current;
            }
        }

        public float NextF
        {
            get
            {
                var t = Next;
                return (float)(t / ((double)uint.MaxValue));
            }
        }

        public bool NextB
        {

            get
            {
                var t = Next;
                return (t & 0x01) == 0;
            }
        }

        public float RangeF(float min, float max)
        {
            var f = NextF;
            f *= max - min;
            return f + min;
        }

        public int RangeI(int min, int max)
        {
            //prevent negative ints during conversion from uint
            int i = (int) (Next % int.MaxValue);

            i %= max - min;
            return i + min;
        }
        
        public static uint XorShift32RandUInt(uint seed)
        {
            var x = seed;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            return x;
        }

        public static float XorShift32RandF(uint seed)
        {
            return (float)(XorShift32RandUInt(seed) / ((double)uint.MaxValue));
        }

        public static bool XorShift32RandB(uint seed)
        {
            return (XorShift32RandUInt(seed) & 0x01) == 0;
        }
    }
}