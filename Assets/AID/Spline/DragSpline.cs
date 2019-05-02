using UnityEngine;
using System.Collections;
namespace AID
{
    public class DragSpline : ABAlongSpline
    {

        public float dragScale = 1f;
        public float minMove = 0, maxMove = 1;
        public Vector3 prevMousePos;


        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                prevMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 inputDif = Input.mousePosition - prevMousePos;
                prevMousePos = Input.mousePosition;

                if (trav.enabled && inputDif.sqrMagnitude > 0)
                {
                    Vector3 splineDir = trav.GetCurrentHeading();
                    splineDir.Normalize();
                    //inputDif.Normalize();

                    float dot = Vector3.Dot(splineDir, inputDif);
                    float dir = dot > 0 ? 1 : -1;

                    dot = Mathf.Clamp(Mathf.Abs(dot), minMove, maxMove);

                    transform.position = trav.Move(dir * dot * dragScale * Time.deltaTime) + offsetPosition;
                }
            }


            base.Update();
        }
    }
}