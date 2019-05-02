using UnityEngine;
using System.Collections;

namespace AID
{

    public class EnableSensor : ABSensor
    {
        public float whileEnabled;
        public bool fireOnEnable, fireOnDisable;

        void OnEnable() { if (fireOnEnable) DetectionOccured(); }
        void OnDisable() { if (fireOnDisable) DetectionOccured(); }
        void Update() { DetectionOccuredLimited(whileEnabled); }
    }

}