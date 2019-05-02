using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace AID
{
    [System.Serializable]
    public class Collider2DUnityEvent : UnityEvent<Collider2D>
    { }
    [System.Serializable]
    public class Collision2DUnityEvent : UnityEvent<Collision2D>
    { }

    public class Collision2DForwarder : MonoBehaviour
    {
        public Collider2DUnityEvent triggerEnter, triggerStay, triggerExit;
        public Collision2DUnityEvent collEnter, collStay, collExit;

        void OnTriggerEnter2D(Collider2D col)
        {
            triggerEnter.Invoke(col);
        }

        void OnTriggerStay2D(Collider2D col)
        {
            triggerStay.Invoke(col);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            triggerExit.Invoke(col);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collEnter.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            collStay.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            collExit.Invoke(collision);
        }
    }
}