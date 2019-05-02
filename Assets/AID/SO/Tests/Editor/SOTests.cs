using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace AID
{
    public class SOTests
    {
        [Test]
        public void SOVarTests()
        {
            const int localStartingVal = 7;
            const int refStartingVal = 10;
            //create a DataTypeVar
            IntVar intv = new IntVar(localStartingVal);
            //test get and set work without SO backing
            Assert.AreEqual(localStartingVal, intv.Value);
            intv.Value += 1;
            Assert.AreEqual(localStartingVal+1, intv.Value);
            intv.Value -= 1;
            Assert.AreEqual(localStartingVal, intv.Value);
            //create SO backing
            var intSOBack = IntSO.CreateInstance<IntSO>();
            intSOBack.Value = refStartingVal;
            intv.SOReferenceValue = intSOBack;
            //test starting value get and set work with backing
            Assert.AreEqual(refStartingVal, intv.Value);
            intv.Value += 1;
            Assert.AreEqual(refStartingVal+1, intv.Value);
            //removing backing and destroy
            intv.SOReferenceValue = null;
            UnityEngine.Object.DestroyImmediate(intSOBack);
            //test local value remains
            Assert.AreEqual(localStartingVal, intv.Value);
        }

        private class EventSOTester : IEventSOListener
        {
            public bool hasBeenFired = false;

            public void OnEventFired(EventSO origin)
            {
                hasBeenFired = true;
            }
        }

        [Test]
        public void SOEventTests()
        {
            //create event
            var ev = EventSO.CreateInstance<EventSO>();
            //add listener
            var evLis = new EventSOTester();
            ev.Add(evLis);
            //invoke
            ev.Fire();
            //assert value matches
            Assert.IsTrue(evLis.hasBeenFired);
            //destroy event
            UnityEngine.Object.DestroyImmediate(ev);
        }

        private class EventSOIntTester : IEventSOParamListener<int>
        {
            public int recVal = 0;

            public void OnEventFired(EventSOParam<int> origin, int param)
            {
                recVal = param;
            }
        }

        [Test]
        public void SOEventParamIntTests()
        {
            const int valToSend = 10;

            //create event
            var ev = EventSOParamInt.CreateInstance<EventSOParamInt>();
            //add listener
            var evLis = new EventSOIntTester();
            ev.Add(evLis);
            //invoke
            ev.Fire(valToSend);
            //assert value matches
            Assert.AreEqual(evLis.recVal, valToSend);
            //destroy event
            UnityEngine.Object.DestroyImmediate(ev);
        }


        [Test]
        public void SOExtVarsTests()
        {
            //Create external vars
            //create csv
            //process

            //assert all values exist and values match

            //destroy external vars
        }
    }
}