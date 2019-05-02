using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sensor for objects that can be interacted with or collected by the player.
 * 
 * Some desired functionality;
 * - Determine objects that can be picked up based on volume infront of player
 * - Prevent picking up through walls
 * - Sort and suggest 'best' item to pickup so it can be shown in the UI
 * - On confirm, notify and collect the 'best item
 * - Show which Item would be pickedup, with text and 'glow'
 */
public class PlayerInteraction : MonoBehaviour
{
    public float reach = 1;
    public float sphereCastRadius = 0.5f;
    public KeyCode pickupKey = KeyCode.E;
    public Transform eyeTransform;
    public LayerMask interactionLayers;
    public InventoryBehaviour inventory;


    private void Update()
    {
        //raycast? to find things we could pickup
        var raycastHits = Physics.SphereCastAll(eyeTransform.position,
                                        sphereCastRadius,
                                        eyeTransform.forward,
                                        reach,
                                        interactionLayers);


        List<InventoryItemBehaviour> hitItems = new List<InventoryItemBehaviour>();

        foreach (var hit in raycastHits)
        {
            var itemBeh = hit.collider.GetComponent<InventoryItemBehaviour>();

            if(itemBeh != null)
            {
                hitItems.Add(itemBeh);
            }
        }

        //this is where we update text or icons or glows

        if(hitItems.Count > 0 && Input.GetKeyDown(pickupKey))
        {
            //pick up
            //put in inventory
            inventory.inventory.Add(hitItems[0].item);
            //delete 
            Destroy(hitItems[0].gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyeTransform.position, sphereCastRadius);
        Gizmos.DrawWireSphere(eyeTransform.position + eyeTransform.forward * reach, sphereCastRadius);
    }
}
