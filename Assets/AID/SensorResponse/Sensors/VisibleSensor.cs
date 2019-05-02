using UnityEngine;
using System.Collections;

namespace AID
{

    /*
     * The problem with visible sensor is at last test the editor scene camera also triggers OnBecameVisible etc.
     * */
    public class VisibleSensor : ABSensor
    {

        public float whileVisible, whileInvisible;
        public bool fireOnBecameVisible, fireOnBecameInvisible;
        private bool vis = false;

        void OnBecameVisible()
        {
            if (fireOnBecameVisible) DetectionOccured();

            vis = true;
        }
        void OnBecameInvisible()
        {
            if (fireOnBecameInvisible) DetectionOccured();

            vis = false;
        }
        void Update()
        {
            DetectionOccuredLimited(vis ? whileVisible : whileInvisible);
        }
    }
}