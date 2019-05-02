using UnityEngine;
using System.Collections;
namespace AID
{
    //takes the difference between an anchor and a spline travelor and emits a re-ranged closeness value between -1 and 1
    //	intended for tweening other objects based on traveler moving based a point on spline
    public class DistBetweenTravelorAndAnchor : MonoBehaviour
    {

        public SplineAnchor anchor;
        public SplineTraveler trav;

        //these vars control the distance before and after the pivot that will be reranged
        //	so a before of -1, 0 and an after of 0,1 applies to reranging
        //		input \/ output \/
        //	limits of -2,-1 1,2
        //		input \/ output \__/
        public float beforeFarLimit = 5, beforeCloseLimit = 0,
                     afterCloseLimit = 0, afterFarLimit = 5;

        public string msgToSend;


        private float previousDist;
        private float previousReRanged;


        void Update()
        {
            //compare splines
            if (trav.spline == anchor.targetSpline)
            {
                float dist = 0;
                //calc distance
                dist = anchor.retPRes.retPos.DistanceBetween(trav.GetReticulatedPosition());
                //Debug.Log(dist);

                if (Mathf.Approximately(dist, previousDist))
                {
                    return;
                }


                previousDist = dist;

                //apply limits and reranges
                if (dist < beforeFarLimit)
                {
                    //too far away
                    dist = -1;
                }
                else if (dist < beforeCloseLimit)
                {
                    //within before range
                    dist = (dist - beforeCloseLimit) / (beforeFarLimit - beforeCloseLimit) * -1;
                }
                else if (dist < afterCloseLimit)
                {
                    //within 0
                    dist = 0;
                }
                else if (dist < afterFarLimit)
                {
                    //within after range
                    dist = (dist - afterCloseLimit) / (afterFarLimit - afterCloseLimit);
                }
                else
                {
                    //beyond after
                    dist = 1;
                }

                //notify
                if (Mathf.Approximately(dist, previousReRanged))
                {
                    return;
                }
                previousReRanged = dist;

                BroadcastMessage(msgToSend, previousReRanged);
            }

        }
    }
}