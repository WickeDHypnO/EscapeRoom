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
    private static KeyCode USE_KEY_CODE = KeyCode.E;


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
            }
            if(targetedItem.GetComponent<HighlightItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
            {
                if(!targetedItem.GetComponent<HighlightItem>().outlineOn)
                {
                    targetedItem.GetComponent<HighlightItem>().OutlineOn();
                }
            }
        }

        if (Input.GetKeyDown(USE_KEY_CODE))
        {
            handleUse();
        }
        else if (Input.GetKey(USE_KEY_CODE) && targetedItem.GetComponent<ConstantUsableTarget>())
        {
            targetedItem.GetComponent<ConstantUsableTarget>().Use();
        }
    }

    private void handleUse()
    {
        if (pickedUpItem)
        {
            pickedUpItem.GetComponent<DraggableItem>().DetachFromPlayer();
            pickedUpItem = null;
        }

        else if (targetedItem.GetComponent<DraggableItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            pickedUpItem = targetedItem;
            Debug.Log(pickedUpItem);
            Debug.Log(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().ChangeOwner(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().AttachToPlayer(photonView.viewID);
        }

        else if (targetedItem.GetComponent<Item>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            GetComponentInParent<PlayerInventory>().AddItem(targetedItem.GetComponent<Item>());
            targetedItem.GetComponent<Collider>().enabled = false;
            targetedItem.GetComponent<HighlightItem>().OutlineOff();
            Destroy(targetedItem.GetComponent<Item>());
            targetedItem = null;
        }

        else if (targetedItem.GetComponent<UsableTarget>())
        {
            targetedItem.GetComponent<UsableTarget>().Use();
        }
    }

}
