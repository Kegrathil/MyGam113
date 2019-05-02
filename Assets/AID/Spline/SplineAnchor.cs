using UnityEngine;
using System.Collections;

namespace AID
{
    //snaps to closest reticulated point at start
    public class SplineAnchor : MonoBehaviour
    {

        public Spline targetSpline;
        public ClosestPointToRetSplineResult retPRes;

        public bool fireOnce = true;

        void Start()
        {
            Snap();


            if (fireOnce)
                enabled = false;
        }

        void Update()
        {
            Snap();
        }

        [ContextMenu("Snap Now")]
        public void Snap()
        {
            if (targetSpline == null)
            {
                Spline[] res = FindObjectsOfType<Spline>();
                if (res.Length == 1)
                    targetSpline = res[0];
            }

            if (targetSpline != null)
            {
                retPRes = targetSpline.CalcClosestPointOnRetSpline(transform.position);
                transform.position = retPRes.closestPoint;
            }
            else
            {
                Debug.LogError("SplineAnchor does not have a spline set");
            }
        }
    }
}