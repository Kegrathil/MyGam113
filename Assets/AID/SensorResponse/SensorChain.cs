using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    //Simple list of ABSensors, acts as an &&
    public class SensorChain : MonoBehaviour
    {

        public List<ABSensor> allOfThese = new List<ABSensor>();
        public List<ABSensor> anyOfThese = new List<ABSensor>();

        void Start() { }

        public bool IsDetected()
        {
            bool retvalAny = false;

            for (int i = 0; i < anyOfThese.Count; i++)
            {
                if (anyOfThese[i] != null && anyOfThese[i].IsDetected())
                {
                    retvalAny = true;
                    break;
                }

            }

            for (int i = 0; i < allOfThese.Count; i++)
            {
                if (!allOfThese[i].IsDetected())
                {
                    return false;
                }
            }

            //we can only get here if all are true or don't exist
            return retvalAny;
        }
    }
}