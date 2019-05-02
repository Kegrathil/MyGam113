using UnityEngine;
using System.Collections;

namespace AID
{
    public class MultiSplineMoveController : ABAlongSpline
    {

        //needs closest point to spline beh
        //but instead of just snapping to point it uses movetopointonspline
        //if the target is one a different spline, then update the traveler to be at that location already

        public ClosestPointToSpline snappedTarget;

        public SplineReticulatedPosition targetPos;
        public float maxSpeed = 1;
        public float closeEnoughTolerance = 0.01f;

        public override void Update()
        {
            if (snappedTarget != null)
            {
                //same spline
                if (trav.spline == snappedTarget.spline)
                {
                    SplineReticulatedPosition cur = trav.GetReticulatedPosition();

                    SplineReticulatedPosition clamped = snappedTarget.GetClamped(cur);

                    float dif = cur.DistanceBetween(clamped);

                    if (Mathf.Abs(dif) > closeEnoughTolerance)
                    {
                        float moveBy = Mathf.Sign(dif) * Mathf.Min(Mathf.Abs(dif), maxSpeed * Time.deltaTime);
                        //	print (dif);

                        transform.position = trav.Move(moveBy);
                    }
                }
                else
                {
                    //its a new spline just set up there
                    trav.SetReticulatedPosition(snappedTarget.closeRes.retPos);
                }
            }
        }
    }
}