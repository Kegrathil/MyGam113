//using UnityEngine;
//using System.Collections;

//namespace AID
//{
//    [System.Serializable]
//    public class MutatorTimeScale : ABMutator
//    {

//        public float timeScale = 1;

//        public override float MutateIncrement(float increment)
//        {
//            return increment * timeScale;
//        }

//        public override float MutateValue(float t)
//        {
//            return t;
//        }
//    }

//    [System.Serializable]
//    public class MutatorReRange : ABMutator
//    {

//        public Vector2 inputRange = new Vector2(0, 1);
//        public Vector2 outputRange = new Vector2(0, 1);

//        public override float MutateIncrement(float increment)
//        {
//            return increment;
//        }

//        public override float MutateValue(float t)
//        {
//            t -= inputRange.x;
//            t /= inputRange.y - inputRange.x;
//            t *= outputRange.y - outputRange.x;
//            t += outputRange.x;
//            return t;
//        }
//    }

//    [System.Serializable]
//    public class MutatorClamp : ABMutator
//    {

//        public Vector2 limits = new Vector2(0, 1);

//        public override float MutateIncrement(float increment)
//        {
//            return increment;
//        }

//        public override float MutateValue(float t)
//        {
//            return Mathf.Clamp(t, limits.x, limits.y);
//        }
//    }
//}