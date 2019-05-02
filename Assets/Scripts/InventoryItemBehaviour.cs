using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * MonoBehaviour that handles the object interactions in scene between the item and other objects
 * 
 * Some desired functionality;
 * - Handles collisions resulting in adding to inventory
 * - Handles triggers resulting in adding to inventory
 * - Handles object being able to be interacted with or picked up
 * 
 * N.B. Requires physics collider to be pickupable
 */
public class InventoryItemBehaviour : MonoBehaviour
{
    public InventoryItemSO item;
}
