using UnityEngine;
using System.Collections;

namespace AID
{
    public static partial class UTIL
    {
        static public Vector3 CubicHermite(Vector3 startTangent, Vector3 start, Vector3 end, Vector3 endTangent, float t)
        {
            float t2 = t * t;
            float t3 = t * t * t;
            float a = 2 * t3 - 3 * t2 + 1;
            float b = t3 - 2 * t2 + t;
            float c = -2 * t3 + 3 * t2;
            float d = t3 - t2;

            return start * a + startTangent * b + end * c + endTangent * d;
        }

        // Cardinal spline takes 4 points and calculates the appropriate in and out tangents, if used on a list of points the spline will pass through all of them
        static public Vector3 Cardinal(Vector3 previous, Vector3 p1, Vector3 p2, Vector3 post, float t)
        {
            //calc tangents
            Vector3 t1 = (p1 - previous) * 0.5f;
            Vector3 t2 = (post - p2) * 0.5f;

            return CubicHermite(t1, p1, p2, t2, t);
        }

    }

}