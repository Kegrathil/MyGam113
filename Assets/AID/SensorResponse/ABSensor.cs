using UnityEngine;
using System.Collections;

namespace AID
{
    public abstract class ABSensor : MonoBehaviour
    {
        //TODO these need to be explained
        public float tippingPoint = 1, attackRate = 1, decayRate = 0.5f, min = 0, max = 1.1f;

        [SerializeField]
        private float cur = 0;
        private bool isDetected = false;

        void Start() { }

        public float Current
        {
            get { return cur; }
            set { cur = value; cur = Mathf.Clamp(cur, min, max); }
        }

        public bool IsDetected()
        {
            bool retval = Current >= tippingPoint;

            //decay now as our true value was just read
            Decay();

            return retval;
        }

        protected void DetectionOccured()
        {
            isDetected = true;
            Current = tippingPoint + Mathf.Epsilon;
        }

        protected void DetectionOccuredLimited(float amt)
        {
            isDetected = true;
            amt = Mathf.Min(amt, attackRate * Time.deltaTime);
            Current += amt;
        }

        protected void Decay()
        {
            if (!isDetected)
                Current -= decayRate * Time.deltaTime;

            isDetected = false;
        }
    }

}