using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    [RequireComponent(typeof(SensorChain))]
    [RequireComponent(typeof(ResponseChain))]
    public class SensorResponseRouter : MonoBehaviour
    {
        public List<SensorChain> anyOfTheseChains = new List<SensorChain>();
        public List<ResponseChain> responses = new List<ResponseChain>();
        public int numberOfExecutionsAllowed = -1;  // -1 means no limit
        private int numTimesExecuted = 0;

        public void FixedUpdate()
        {
            bool anyOfThem = false;

            foreach (SensorChain sc in anyOfTheseChains)
            {
                if (sc.IsDetected())
                {
                    anyOfThem = true;
                    break;
                }
            }

            if (!anyOfThem)
                return;

            if (numTimesExecuted >= numberOfExecutionsAllowed && numberOfExecutionsAllowed > 0)
                return;

            numTimesExecuted++;

            foreach (ResponseChain r in responses)
            {
                r.Fire(this);
            }
        }
    }
}