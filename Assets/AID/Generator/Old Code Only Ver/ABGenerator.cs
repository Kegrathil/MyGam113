//using UnityEngine;
//using System.Collections;
//using System.Xml.Serialization;

////TODO 
////	this needs an editor, could be node based, could be heirarchy, but needs something
////	expanders, that turn float into something else

//namespace AID
//{
//    /*
//        Generators and simple classes that lock together to form procedural values information. Think of them
//        as a way to create complex highly tweakable (at runtime if you wish) AnimationCurves.
//      */
//    [System.Serializable]
//    public abstract class ABGenerator : System.Object
//    {

//        private float t = 1;    //where is this generator up to in its life cycle
//        protected float period = 1; //where is this generator up to in its life cycle

//        [SerializeField]
//        private WrapMode mode = WrapMode.PingPong;
//        public WrapMode Mode
//        {
//            get { return mode; }

//            set
//            {
//                if (value == WrapMode.Loop || value == WrapMode.PingPong)
//                    mode = value;
//                else
//                    Debug.Log("ABGenerator Mode can only be Loop or PingPong");
//            }
//        }

//        public virtual float Generate(float increment)
//        {
//            return GenerateInternal(IncrementAndWrap(increment));
//        }

//        //Sub classes use given t and return appropriate data
//        protected abstract float GenerateInternal(float time);

//        public float IncrementAndWrap(float increment)
//        {
//            t += increment;

//            float retval = t;

//            if (t > period)
//            {
//                if (Mode == WrapMode.Loop)
//                {
//                    t %= period;
//                    retval = t;
//                }
//                else if (Mode == WrapMode.PingPong)
//                {
//                    t %= period * 2;
//                    retval = Mathf.PingPong(t, period);
//                }
//            }

//            return retval;
//        }
//    }
//}