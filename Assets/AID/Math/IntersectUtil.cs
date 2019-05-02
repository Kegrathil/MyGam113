
using UnityEngine;

namespace AID
{
    public struct Circle3D
    {
        public Plane plane;
        public Vector3 centre;
        public float radius;
    };

    static public partial class UTIL
    {


        /*
	     * Calculate a circle given three points.
	     */
        static public Circle3D CalculateCircleFrom3Points2D(Vector3 p0r, Vector3 p1r, Vector3 p2r)
        {
            Circle3D circ = new Circle3D();

            //fix winding if we can
            if (p2r.x < p1r.x)
            {
                Vector3 temp = p1r;
                p1r = p2r;
                p2r = temp;
            }

            Vector3 l0 = p0r - p1r;
            //l0.z = 0;
            Vector3 l1 = p2r - p1r;
            //l1.z = 0;

            //perp the lines and find their intersect to find circle centre
            Vector3 l0perp = new Vector3(l0.y, -l0.x, 0);
            Vector3 l1perp = new Vector3(l1.y, -l1.x, 0);

            Vector3 l0perpStart = p1r + l0 * 0.5f;
            Vector3 l1perpStart = p1r + l1 * 0.5f;

            circ.centre = LineLineIntersect2D(l0perpStart, l0perp, l1perpStart, l1perp);
            circ.plane = new Plane(new Vector3(0, 0, -1), 0);
            Vector3 aPoint = p0r;
            aPoint.z = 0;
            circ.radius = (circ.centre - aPoint).magnitude;

            return circ;
        }

        /*
	     * Project 3d point onto 2d plane. Out is relative to the plane, z is the distnance
	     * from the plane
	     */
        static public Vector3 Project3DOntoPlane(Vector3 p, Plane plane)
        {
            //we need a point on the plane
            //TODO this could be a better guess of a point, this is projecting 0,0,0 as the plane points
            float dist = plane.GetDistanceToPoint(p);
            Vector3 planePoint = plane.normal * dist;
            //make it 2d relative to the plane
            p = p - planePoint;
            p = Quaternion.FromToRotation(plane.normal, new Vector3(0, 0, 1)) * p;

            p.z = dist;
            return p;
        }


        /*
	     * 2d (x,y) line intersect, intersect may be beyond the ends of the segment
	     */
        static public Vector2 LineLineIntersect2D(Vector3 l0p, Vector3 l0n, Vector3 l1p, Vector3 l1n)
        {
            //2d cross of line normals
            float denom = l0n.y * l1n.x - l0n.x * l1n.y;

            if (Mathf.Approximately(denom, 0))
            {
                return new Vector2(99999999999999999999.0f, 999999999999999.0f);
            }

            //2d cross of second normal and dif in starts
            float numer = (l0p - l1p).x * l1n.y - (l0p - l1p).y * l1n.x;

            float dist = numer / denom;

            return l0p + l0n * dist;
        }

        
    }
}
