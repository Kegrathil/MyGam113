//using UnityEngine;
//using System.Collections;

//namespace AID
//{
//    [System.Serializable]
//    public class GeneratorRandom : ABGenerator
//    {
//        protected override float GenerateInternal(float t)
//        {
//            return Random.value;
//        }
//    }

//    [System.Serializable]
//    public class GeneratorPerlin : ABGenerator
//    {

//        public float seed;

//        public GeneratorPerlin()
//        {
//            period = 10;
//            seed = Random.Range(0.0f, 10.0f);
//        }

//        protected override float GenerateInternal(float t)
//        {
//            return Mathf.PerlinNoise(seed, t);
//        }
//    }

//    [System.Serializable]
//    public class GeneratorSin : ABGenerator
//    {

//        public GeneratorSin()
//        {
//            period = Mathf.PI * 2;
//            Mode = WrapMode.Loop;
//        }

//        protected override float GenerateInternal(float t)
//        {
//            return Mathf.Sin(t);
//        }
//    }

//    [System.Serializable]
//    public class GeneratorCurve : ABGenerator
//    {

//        public AnimationCurve curve;

//        protected override float GenerateInternal(float t)
//        {
//            return curve.Evaluate(t);
//        }
//    }
//}