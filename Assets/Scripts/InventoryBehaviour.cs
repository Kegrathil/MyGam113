using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * MonoBehaviour that handles an object having a inventory that it owns or has direct access to
 * 
 * Some desired functionality;
 * - Other objects can rely on GOs that have an inventory having one of these
 */
public class InventoryBehaviour : MonoBehaviour
{
    public bool clearOnStart = true;

    private void Start()
    {
        if (clearOnStart)
        {
            inventory.items.Clear();
        }
    }

    public InventorySO inventory;
}
