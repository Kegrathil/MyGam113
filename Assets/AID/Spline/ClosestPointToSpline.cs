using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    public class ClosestPointToSpline : MonoBehaviour
    {

        public Spline spline;
        public List<SplineJunction> splineJunctions;
        public List<SplineGate> splineGates;
        public ClosestPointToRetSplineResult closeRes;
        public float speed = 1;

        public float dragScale = 0.001f;
        public Vector3 prevMousePos;

        public bool syncPosToResult = false;


        public bool allowPlayerControl = true;
        public bool isForcedTo = false;
        public Vector3 forceToPos;
        public int numConsideredSplines = 0;

        public Transform startingFrom;
        public SplineTraveler trav;
        // Use this for initialization
        void Start()
        {

        }

        public SplineReticulatedPosition GetClamped(SplineReticulatedPosition cur)
        {
            SplineReticulatedPosition retval = closeRes != null ? closeRes.retPos : null;

            foreach (SplineGate sg in splineGates)
            {
                if (sg.enabled && sg.gameObject.activeSelf && sg.gameObject.activeInHierarchy)
                    retval = sg.ProcessCrossingRequest(cur, retval);
            }

            return retval;
        }

        public void ForceTo(Vector3 pos)
        {
            forceToPos = pos;
            isForcedTo = true;
        }

        // Update is called once per frame
        void Update()
        {

            if (isForcedTo)
            {
                MakeClosestTo(forceToPos);
                if ((closeRes.closestPoint - GetStartingPosition()).magnitude < 0.1f)
                    isForcedTo = false;//made it
            }
            else if (allowPlayerControl)
            {
                PlayerControlledMakeClosestTo();
            }

        }

        void OnTriggerEnter(Collider col)
        {
            SplineJunction sj = col.GetComponent<SplineJunction>();
            if (sj != null) splineJunctions.Add(sj);

            SplineGate sg = col.GetComponent<SplineGate>();
            if (sg != null) splineGates.Add(sg);

            splineJunctions.RemoveAll(item => item == null);
            splineGates.RemoveAll(item => item == null);
        }

        void OnTriggerExit(Collider col)
        {
            SplineJunction sj = col.GetComponent<SplineJunction>();
            if (sj != null) splineJunctions.Remove(sj);

            SplineGate sg = col.GetComponent<SplineGate>();
            if (sg != null) splineGates.Remove(sg);

            splineJunctions.RemoveAll(item => item == null);
            splineGates.RemoveAll(item => item == null);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        private Vector3 GetStartingPosition()
        {
            return startingFrom != null ? startingFrom.position : transform.position;
        }

        private void PlayerControlledMakeClosestTo()
        {
            Vector3 desiredPos = GetStartingPosition();
            Vector3 inputDir = Vector3.zero, inputDirRaw;

            inputDirRaw = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            inputDir += inputDirRaw * Time.deltaTime * speed;

            //print(inputDir);

            if (Input.GetMouseButtonDown(0))
            {
                prevMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 curInput = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector3 inputDif = (curInput - prevMousePos) * dragScale;
                inputDir += inputDif;
                //print (inputDif);
                prevMousePos = curInput;
            }

            //constraint and readjust inputDir
            Vector3 splineDir = Vector3.up;
            if (trav != null && splineJunctions.Count < 1 && !Mathf.Approximately(0.0f, inputDirRaw.magnitude))
            {
                splineDir = trav.GetCurrentHeading();
                splineDir.Normalize();

                float dot = Vector3.Dot(splineDir, inputDir.normalized);
                float dir = dot > 0 ? 1 : -1;

                inputDir = splineDir * dir * Time.deltaTime * speed;
            }//inputDif.Normalize();

            desiredPos += inputDir;


            MakeClosestTo(desiredPos);
        }

        private void MakeClosestTo(Vector3 desiredPos)
        {
            numConsideredSplines = 0;
            if (splineJunctions.Count > 0)
            {
                //find closest among spline junctions
                ClosestPointToRetSplineResult res = null, curBest = null;
                Spline closestSpline = null;

                foreach (SplineJunction sj in splineJunctions)
                {
                    foreach (Spline s in sj.splines)
                    {
                        numConsideredSplines++;
                        res = s.CalcClosestPointOnRetSpline(desiredPos);

                        if (curBest == null || curBest.distSqrFromTarget > res.distSqrFromTarget)
                        {
                            curBest = res;
                            closestSpline = s;
                        }
                    }
                }

                spline = closestSpline;
                closeRes = curBest;
            }
            else if (spline != null)
            {
                closeRes = spline.CalcClosestPointOnRetSpline(desiredPos);
            }
            else
            {
                Debug.LogError("Cannot find closest when there are no splines");
                closeRes = null;
            }

            if (syncPosToResult && closeRes != null)
            {
                transform.position = closeRes.closestPoint;
            }
        }
    }
}