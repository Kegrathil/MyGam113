using UnityEngine;
using System.Collections;
using UnityEngine.Events;


namespace AID
{
    [System.Serializable]
    public class ColliderUnityEvent : UnityEvent<Collider>
    { }
    [System.Serializable]
    public class CollisionUnityEvent : UnityEvent<Collision>
    { }

    public class CollisionForwarder : MonoBehaviour
    {
        public ColliderUnityEvent triggerEnter, triggerStay, triggerExit;
        public CollisionUnityEvent collEnter, collStay, collExit;

        void OnTriggerEnter(Collider col)
        {
            triggerEnter.Invoke(col);
        }

        void OnTriggerStay(Collider col)
        {
            triggerStay.Invoke(col);
        }

        void OnTriggerExit(Collider col)
        {
            triggerExit.Invoke(col);
        }

        private void OnCollisionEnter(Collision collision)
        {
            collEnter.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            collStay.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            collExit.Invoke(collision);
        }
    }
}