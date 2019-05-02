using UnityEngine;
using System.Collections;
namespace AID
{
    public class ABAlongSpline : MonoBehaviour
    {

        public SplineTraveler trav;
        public Vector3 offsetPosition;
        public GenerateOffsetMode offsetMode = GenerateOffsetMode.User;
        public bool shouldFetchRotation = false;


        public void OnEnable()
        {
            trav = GetComponent<SplineTraveler>();
            if (trav == null)
            {
                Debug.LogError("No SplineTraveler");
            }

            switch (offsetMode)
            {
                case GenerateOffsetMode.OnEnableRelativeOrigin:
                    offsetPosition = transform.position;
                    break;

                case GenerateOffsetMode.OnEnableRelativeFirstNode:
                    Transform nodeZeroTrans = trav.spline.GetNode(0).transform;
                    offsetPosition = transform.position - nodeZeroTrans.position;
                    break;
            }
        }

        public virtual void Update()
        {
            if (trav.enabled)
            {
                if (shouldFetchRotation)
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, trav.GetCurrentHeading().normalized);
            }
        }

    }

    public enum GenerateOffsetMode
    {
        User,
        OnEnableRelativeOrigin,
        OnEnableRelativeFirstNode,
    };
}