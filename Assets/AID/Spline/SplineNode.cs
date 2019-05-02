using UnityEngine;
using System.Collections;
namespace AID
{
    //TODO
    //	retic splines should not split on distance but on variance in angle so we can bunch up samples on sharp turns

    public class SplineNode : MonoBehaviour
    {


        public Vector3 inFragment, outFragment; //half the equation of the TB tangent
        public float tension = 0;   //taughtness of the rope
        public float bias = 0;      //negative bends early, positive bends late
        public SplineTangentInfo inInfo = new SplineTangentInfo(), outInfo = new SplineTangentInfo();
        public Bounds bounds = new Bounds();
        public float distanceFromStartOfSpline = 0;
        public float distanceToNextNode = -1;

        public Vector3[] reticulationPoints = new Vector3[0];
        public ReticulatedSplineSegment[] reticulatedSegments = new ReticulatedSplineSegment[0];

        public int GetNumReticulatedSegments()
        {
            return reticulatedSegments.Length;
        }

        public ReticulatedSplineSegment GetReticulatedSegment(int seg)
        {
            if (seg >= reticulatedSegments.Length || seg < 0)
            {
                return null;
            }

            return reticulatedSegments[seg];
        }

        public void Reticulate(SplineNode nextNode, int resolution)
        {
            ZeroReticulatedData();
            resolution = Mathf.Max(2, resolution);
            reticulationPoints = new Vector3[resolution];

            Vector3 t1 = inFragment + outFragment;
            Vector3 t2 = nextNode.inFragment + nextNode.outFragment;
            Vector3 p1 = transform.position;
            Vector3 p2 = nextNode.transform.position;

            float increment = 1.0f / (resolution - 1);

            bounds = new Bounds();

            for (int i = 0; i < resolution; ++i)
            {
                reticulationPoints[i] = AID.UTIL.CubicHermite(t1, p1, p2, t2, increment * i);
                bounds.Encapsulate(reticulationPoints[i]);
            }

            distanceToNextNode = 0;
            reticulatedSegments = new ReticulatedSplineSegment[resolution - 1];
            for (int i = 0; i < reticulatedSegments.Length; ++i)
            {
                ReticulatedSplineSegment retseg = new ReticulatedSplineSegment();

                retseg.startPos = reticulationPoints[i];
                retseg.endPos = reticulationPoints[i + 1];


                retseg.dir = retseg.endPos - retseg.startPos;
                retseg.length = retseg.dir.magnitude;
                retseg.dir.Normalize();

                reticulatedSegments[i] = retseg;

                distanceToNextNode += retseg.length;
            }
        }

        public void ZeroReticulatedData()
        {
            reticulationPoints = null;
            reticulatedSegments = null;
            distanceToNextNode = 0;
        }

        public void CalcuateTangentInfo(SplineNode prev, SplineNode next)
        {
            inFragment = Vector3.zero;
            outFragment = Vector3.zero;

            //if the prev is us then no inTan
            if (prev != this)
            {
                //using 2 tension as default should be 1, but that makes little sense
                inFragment = inInfo.GetTangent(prev.transform.position, this.transform.position) * ((2 - tension) * (1 + bias));
            }

            //if next is us then no outTan
            if (next != this)
            {
                outFragment = outInfo.GetTangent(this.transform.position, next.transform.position) * ((2 - tension) * (1 - bias));
            }

        }
    }


    public enum SplineTangetMode
    {
        Percentage,
        Fixed
    };

    [System.Serializable]
    public class SplineTangentInfo
    {
        public SplineTangetMode mode = SplineTangetMode.Percentage;
        public float ammount = 0.25f;

        public Vector3 GetTangent(Vector3 p1, Vector3 p2)
        {
            Vector3 dif = p2 - p1;
            switch (mode)
            {
                case SplineTangetMode.Percentage:
                    float dist = dif.magnitude;
                    dif = dif.normalized * dist * ammount;
                    break;

                case SplineTangetMode.Fixed:
                    dif = dif.normalized * ammount;
                    break;
            }

            return dif;
        }
    };

    [System.Serializable]
    public class ReticulatedSplineSegment
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public Vector3 dir;
        public float length;
    };
}