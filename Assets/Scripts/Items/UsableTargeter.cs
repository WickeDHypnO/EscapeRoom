using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableTargeter : Photon.PunBehaviour
{

    public LayerMask raycastMask;
    public float itemPickupDistance = 2f;
    RaycastHit hitInfo;
    GameObject targetedItem;
    GameObject pickedUpItem;

    void LateUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 50f, raycastMask))
        {
            if (targetedItem != hitInfo.collider.gameObject)
            {
                if (targetedItem && targetedItem.GetComponent<HighlightItem>())
                {
                    targetedItem.GetComponent<HighlightItem>().OutlineOff();
                }
                targetedItem = hitInfo.collider.gameObject;

                if (targetedItem.GetComponent<HighlightItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
                {
                    targetedItem.GetComponent<HighlightItem>().OutlineOn();
                }
            }
            if(targetedItem.GetComponent<HighlightItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
            {
                if(!targetedItem.GetComponent<HighlightItem>().outlineOn)
                {
                    targetedItem.GetComponent<HighlightItem>().OutlineOn();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && pickedUpItem)
        {
            pickedUpItem.GetComponent<DraggableItem>().DetachFromPlayer();
            pickedUpItem = null;
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<DraggableItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            pickedUpItem = targetedItem;
            Debug.Log(pickedUpItem);
            Debug.Log(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().ChangeOwner(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().AttachToPlayer(photonView.viewID);
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<Item>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            GetComponentInParent<PlayerInventory>().AddItem(targetedItem.GetComponent<Item>());
            targetedItem.GetComponent<Collider>().enabled = false;
            targetedItem.GetComponent<HighlightItem>().OutlineOff();
            targetedItem = null;
            return; // ?? inaczej niżej się wysypuje
        }

        if(Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<LeverController>())
        {
            targetedItem.GetComponent<LeverController>().Use();
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<ButtonController>())
        {
            targetedItem.GetComponent<ButtonController>().Use();
        }
    }

}
