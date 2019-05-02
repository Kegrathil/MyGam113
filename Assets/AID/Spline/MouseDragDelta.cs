using UnityEngine;
using System.Collections;

namespace AID
{
    /*
        Reusable helper for calculating mouse deltas during a drag
    */
    public class MouseDragDelta : MonoBehaviour
    {

        private Vector3 prevMousePos;
        public bool isMouseDown = false;

        public Vector3 deltaPixels, deltaView;

        void OnMouseDown()
        {
            prevMousePos = Input.mousePosition;
            isMouseDown = true;
        }

        void OnMouseDrag()
        {
            deltaPixels = Input.mousePosition - prevMousePos;
            prevMousePos = Input.mousePosition;
            deltaView = deltaPixels;
            deltaView.Scale(new Vector3(1.0f / Screen.width, 1.0f / Screen.height, 1));
        }

        void OnMouseUp()
        {
            isMouseDown = false;
            deltaPixels = Vector3.zero;
            deltaView = Vector3.zero;
        }
    }
}