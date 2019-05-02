using UnityEngine;
using System.Collections;
namespace AID
{
    public enum SplineGateMode
    {
        NoGreater,
        NoLess,
        NoCrossing
    }

    public class SplineGate : MonoBehaviour
    {

        public SplineReticulatedPosition pos;
        public SplineGateMode mode;


        void Start()
        {
            SplineAnchor an = GetComponent<SplineAnchor>();

            if (an != null)
            {
                pos = an.retPRes.retPos;
            }
        }

        public SplineReticulatedPosition ProcessCrossingRequest(SplineReticulatedPosition cur, SplineReticulatedPosition desired)
        {
            //determine if in range
            float curDist = pos.DistanceBetween(cur);
            float desDist = pos.DistanceBetween(desired);

            if ((curDist * desDist) > 0)
            {
                //they are on the same side of us, so we aren't in the way of them
                return desired;
            }

            //determine direction of travel
            //does that match our mandate
            //clamp the ret pos with a little bit of wiggle room

            SplineReticulatedPosition retval = new SplineReticulatedPosition();
            retval.CopyVals(desired);

            if (curDist < 0 && (mode == SplineGateMode.NoGreater || mode == SplineGateMode.NoCrossing))
            {
                retval.CopyVals(pos);
                retval.distanceFromRetStart *= 0.999f;
                retval.distanceFromRetStart -= float.Epsilon;
            }
            else if (curDist > 0 && (mode == SplineGateMode.NoLess || mode == SplineGateMode.NoCrossing))
            {
                retval.CopyVals(pos);
                retval.distanceFromRetStart *= 1.001f;
                retval.distanceFromRetStart += float.Epsilon;
            }

            return retval;
        }
    }
}