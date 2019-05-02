using UnityEngine;
using System.Collections;

namespace AID
{
    public class FacingSensor : ABSensor
    {

        public Transform desiredFacing;
        public Transform targetObject;
        public float withinAngle = 45;
        public float lessThanRate, greaterThanRate;

        void FixedUpdate()
        {
            float ang = Vector3.Angle(targetObject.forward, desiredFacing.forward);
            DetectionOccuredLimited(ang < withinAngle ? lessThanRate : greaterThanRate);
        }
    }
}