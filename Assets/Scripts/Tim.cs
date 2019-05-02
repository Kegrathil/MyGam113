using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a tag component.
/// </summary>
public class Tim : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Door door = other.GetComponent<Door>();
        if(door != null)
        {
            door.close = true;
        }
    }
}
