using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    static public class ProNav
    {
        /*
            Uses proportial navigation to return the acceleration a perfect and infinitely powerful missile should take
            to intercept its target. http://en.wikipedia.org/wiki/Proportional_navigation
            */
        public static Vector3 CalcAccel(Vector3 targetPos, Vector3 targetVel, Vector3 missilePos, Vector3 missileVel, float proNavConstant)
        {
            Vector3 LOS = targetPos - missilePos;

            Vector3 closingVel = targetVel - missileVel;

            Vector3 rotVec = Vector3.Cross(LOS, closingVel) / Vector3.Dot(LOS, LOS);

            Vector3 accel = Vector3.Cross(proNavConstant * closingVel, rotVec);

            return accel;
        }
    }
}