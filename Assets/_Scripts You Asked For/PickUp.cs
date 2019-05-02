using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask pickupMask;

    [SerializeField] private KeyCode key;

    [SerializeField] private Vector3 offset;

    private GameObject curObject;

    private CharacterController cc;
    private Vector3 normPos;
    private float normRadius;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        normPos = cc.center;
        normRadius = cc.radius;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, pickupMask))
            {
                curObject = hit.collider.gameObject;

                curObject.transform.parent = transform;
                curObject.transform.localPosition = offset;
                curObject.transform.localEulerAngles = new Vector3(0, 0, 0);

                cc.center = new Vector3(cc.center.x, cc.center.y, cc.center.z + 0.7f);
                cc.radius *= 2;

                Rigidbody rb = curObject.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
        if (Input.GetKeyUp(key) && curObject != null)
        {
            curObject.transform.parent = null;

            cc.center = normPos;
            cc.radius = normRadius;

            Rigidbody rb = curObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            curObject = null;
        }
    }
}
