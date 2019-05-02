using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Items can be stored in the inventory, they can be either physical assets
 * or concepts. For example you could grant the player an 'item' when they 
 * complete a puzzle or read a note, serving as a token to allow doors
 * to check for its presence in inventory before opening.
 * 
 * See also InventorySO
 * 
 * Some desired functionality;
 * - Optional visual representation for use in world
 * - Optional visual representation for use in UI
 */
[CreateAssetMenu()]
public class InventoryItemSO : ScriptableObject
{
}
