//using UnityEngine;
//using System.Collections;

//namespace AID
//{
//    /*
//		Mutators take a Generator and manipulate the incoming data and return it as a generator themselves.
//		They behave like decorators.
//	*/
//    [System.Serializable]
//    public abstract class ABMutator : ABGenerator
//    {

//        public ABGenerator childGen;

//        //on generate, manip time, call gen on child, manip res	
//        public override float Generate(float increment)
//        {
//            increment = MutateIncrement(increment);
//            return MutateValue(childGen.Generate(increment));
//        }

//        public abstract float MutateIncrement(float increment);
//        public abstract float MutateValue(float t);


//        //unused
//        protected override float GenerateInternal(float time) { return 0; }
//    }
//}