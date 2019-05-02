using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float closeSpeed;
    [SerializeField] private float closedRotation;

    [HideInInspector] public bool close;

    private void FixedUpdate()
    {
        if (close)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, closedRotation, 0), closeSpeed * Time.deltaTime);
        }
    }
}
