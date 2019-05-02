using System;

namespace AID
{

    public class ThreadLocalRandom
    {
        private static readonly System.Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        private static Random Inst
        {
            get
            {
                if (_local == null)
                {
                    int seed;
                    lock (_global)
                    {
                        seed = _global.Next();
                    }
                    _local = new Random(seed);
                }
                return _local;
            }
        }

        public static int Next()
        {
            return Inst.Next();
        }

        public static double Sample()
        {
            return Inst.NextDouble();
        }

        public static float SampleF()
        {
            return (float)Sample();
        }

        public static bool SampleB()
        {
            //int from 0-max  > maxint/2
            return Next() > (int.MaxValue >> 1);
        }
    }
}