using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An Inventory can hold any number of InventoryItems. These are both SOs. 
 * These are used in controllers and behaviours on gameobjects or by other SOs.
 * 
 * The goal here is to allow the creation of simple scripts that add, remove, or check
 * for the presence of certain inventory items and take actions based on upon the result.
 * 
 * Some desirable functionality;
 * - Colliding with an item adds to it to the inventory of the thing colliding with it
 * - 'Interacting' with object adds item(s) to inventory of thing interacting with it
 * - 'Interacting' with object checks for presence of item(s) in inventory runs branching
 *      logic based on result
 * - Colliding with "
 * - Notify subscribers when the inventory changes, add or remove an item
 */
 [CreateAssetMenu()]
public class InventorySO : ScriptableObject
{
    public List<InventoryItemSO> items = new List<InventoryItemSO>();

    public AID.EventSO OnChanged;

    public void Add(InventoryItemSO item)
    {
        items.Add(item);
        if(OnChanged != null)
            OnChanged.Fire();
    }

    public void Remove(InventoryItemSO item)
    {
        items.Remove(item);
        if (OnChanged != null)
            OnChanged.Fire();
    }
}
