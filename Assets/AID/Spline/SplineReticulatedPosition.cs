using UnityEngine;
using System.Collections;

//TODO
//	cache final pos for multiple comparisons
namespace AID
{
    [System.Serializable]
    public class SplineReticulatedPosition
    {
        public Spline spline;
        public int splineNodeIndex;
        public int retSeg;
        public float distanceFromRetStart;

        public void CopyVals(SplineReticulatedPosition rhs)
        {
            spline = rhs.spline;
            splineNodeIndex = rhs.splineNodeIndex;
            retSeg = rhs.retSeg;
            distanceFromRetStart = rhs.distanceFromRetStart;
        }

        public float DistanceBetween(SplineReticulatedPosition other)
        {
            //choose closest
            if (other == null || this.spline != other.spline)
                return float.MaxValue;

            float sign = 1f;

            SplineReticulatedPosition near = null, far = null;

            if (this.splineNodeIndex < other.splineNodeIndex)
            {
                near = this;
                far = other;
            }
            else if (this.splineNodeIndex > other.splineNodeIndex)
            {
                far = this;
                near = other;
                sign = -1;
            }
            else if (this.retSeg < other.retSeg)
            {
                near = this;
                far = other;
            }
            else if (this.retSeg > other.retSeg)
            {
                far = this;
                near = other;
                sign = -1;
            }
            else//same node and ret seg
            {
                //if start seg and end seg the same return dif in distance traveled
                return other.distanceFromRetStart - this.distanceFromRetStart;
            }

            //add rem distance in this seg
            float distAccum = near.spline.GetNode(near.splineNodeIndex).GetReticulatedSegment(near.retSeg).length - near.distanceFromRetStart;
            //add the distance the final seg as already traveled
            distAccum += far.distanceFromRetStart;

            int curIndex = near.splineNodeIndex, curSeg = near.retSeg + 1;

            //run until we are on the same node and same seg
            while (curIndex < far.splineNodeIndex || curSeg < far.retSeg)
            {
                ReticulatedSplineSegment ret = near.spline.GetNode(curIndex).GetReticulatedSegment(curSeg);

                if (ret != null)
                {
                    distAccum += ret.length;
                    //move from its seg forward 
                    curSeg++;
                }
                else
                {
                    //end of ret info move forward
                    curIndex++;
                    curSeg = 0;
                }
            }

            return distAccum * sign;
        }
    }

    [System.Serializable]
    public class ClosestPointToRetSplineResult
    {
        public Vector3 closestPoint;
        public float distSqrFromTarget = float.MaxValue;
        public SplineReticulatedPosition retPos;
    }

}