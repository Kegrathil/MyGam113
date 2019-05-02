using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AID
{
    public class CartesianPointAngle
    {
        public CartesianPointAngle(Vector2 inp)
        {
            p = inp;
        }

        public Vector2 p;
        public float a;
    };

    public class GrahamScanHull
    {
        /*
         * Takes a collection of 2d vectors and returns an ordered list of vectors that are
         * the hull
         */
        static public System.Collections.Generic.List<Vector3> GrahamScan2DConvexHull(System.Collections.Generic.List<Vector3> input)
        {
            GrahamScanHull gsh = new GrahamScanHull();

            foreach (Vector3 p in input)
            {
                gsh.AddPoint(p);
            }

            gsh.Process();

            System.Collections.Generic.List<Vector3> res = new System.Collections.Generic.List<Vector3>();

            foreach (Vector2 p in gsh.hull)
            {
                res.Add(p);
            }

            return res;
        }

        static public System.Collections.Generic.List<Vector2> GrahamScan2DConvexHull(System.Collections.Generic.List<Vector2> input)
        {
            GrahamScanHull gsh = new GrahamScanHull();

            foreach (Vector2 p in input)
            {
                gsh.AddPoint(p);
            }

            gsh.Process();

            return gsh.hull;
        }


        public List<Vector2> inputPoints;
        public List<Vector2> hull;
        public List<CartesianPointAngle> sorted;

        public GrahamScanHull()
        {
            Reset();
        }

        public void Reset()
        {
            inputPoints = new List<Vector2>();
            hull = new List<Vector2>();
            sorted = new List<CartesianPointAngle>();
        }

        public void AddPoint(Vector3 p)
        {
            inputPoints.Add(new Vector2(p.x, p.y));
        }

        public void AddPoint(Vector2 p)
        {
            inputPoints.Add(p);
        }

        public void Process()
        {
            if (inputPoints.Count <= 3)
            {
                hull = inputPoints;
                return;
            }

            //find the anchor
            Sort();

            int j = 0;
            for (; j < 3; ++j)
            {
                hull.Add(sorted[j].p);
            }

            for (; j < sorted.Count; ++j)
            {
                //check for invalid turns
                while (hull.Count > 2 && !LeftTurn2d(hull[hull.Count - 2], hull[hull.Count - 1], sorted[j].p))
                {
                    hull.RemoveAt(hull.Count - 1);
                }

                //add the next point
                hull.Add(sorted[j].p);
            }
        }

        private void Sort()
        {
            int anchorIndex = 0;

            for (int i = 0; i < inputPoints.Count; i++)
            {

                sorted.Add(new CartesianPointAngle(inputPoints[i]));

                //find lowest point
                if (sorted[anchorIndex].p.y >= sorted[i].p.y)
                    anchorIndex = i;
            }

            //move anchor to 0
            if (sorted.Count <= anchorIndex)
            {
                Debug.Log("wtf: " + anchorIndex.ToString() + ", " + sorted.Count.ToString());
            }

            CartesianPointAngle tmp = sorted[anchorIndex];
            sorted[anchorIndex] = sorted[0];
            sorted[0] = tmp;

            for (int i = 1; i < sorted.Count; ++i)
            {
                sorted[i].a = Mathf.Atan2(sorted[i].p.y - sorted[0].p.y, sorted[i].p.x - sorted[0].p.x);
            }

            //sort that
            sorted.Sort(CompareCartesianPointAngle);
        }

        static private int CompareCartesianPointAngle(CartesianPointAngle lhs, CartesianPointAngle rhs)
        {
            return lhs.a < rhs.a ? -1 : (lhs.a > rhs.a ? 1 : 0);
        }

        //2d xy, check
        static public bool LeftTurn2d(Vector3 a, Vector3 b, Vector3 c)
        {
            //(x2 − x1)(y3 − y1) − (y2 − y1)(x3 − x1)
            return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x) > 0.0f;
        }
    }
}