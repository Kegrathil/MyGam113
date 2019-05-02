//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

////inher doesn't work so what about delegates?

//namespace AID
//{
//    /*
//		Collectors simply have a list of Generators, sum them and return.
		
//		At some poin in the future it may be required to make this a base and allow more complex operations such as average, mode, normalise etc.
//	*/
//    [System.Serializable]
//    public class Collector : ABGenerator
//    {

//        public bool average = false;

//        public List<ABGenerator> children = new List<ABGenerator>();

//        //on generate, manip time, call gen on child, manip res	
//        public override float Generate(float increment)
//        {
//            float retval = 0;

//            for (int i = 0; i < children.Count; i++)
//            {
//                retval += children[i].Generate(increment);
//            }

//            if (average)
//                retval /= children.Count;

//            return retval;
//        }
//        //unused
//        protected override float GenerateInternal(float time) { return 0; }

//    }
//}
