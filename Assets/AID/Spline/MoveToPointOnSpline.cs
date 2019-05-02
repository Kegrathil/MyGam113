using UnityEngine;
using System.Collections;

//have a desired spline position to be at and a method of getting there
namespace AID
{
    public class MoveToPointOnSpline : ABAlongSpline
    {

        public SplineReticulatedPosition targetPos;
        public float maxSpeed = 1;
        public float closeEnoughTolerance = 0.01f;

        public override void Update()
        {
            if (targetPos != null)
            {
                SplineReticulatedPosition cur = trav.GetReticulatedPosition();

                float dif = cur.DistanceBetween(targetPos);

                if (Mathf.Abs(dif) > closeEnoughTolerance)
                {
                    float moveBy = Mathf.Sign(dif) * Mathf.Min(Mathf.Abs(dif), maxSpeed * Time.deltaTime);
                    //print (dif);

                    transform.position = trav.Move(moveBy);
                }
            }
        }

    }
}