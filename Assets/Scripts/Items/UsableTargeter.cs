using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableTargeter : Photon.PunBehaviour
{
    public const float DefaultItemUseDistance = 2.0f;

    public LayerMask raycastMask;
    public float itemPickupDistance = 2f;
    RaycastHit hitInfo;
    GameObject targetedItem;
    GameObject pickedUpItem;
    private static KeyCode USE_KEY_CODE = KeyCode.Mouse0;


    void LateUpdate()
    {
        bool distanceCondition = false;
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
            bool isUsable = targetedItem.GetComponent<UsableTarget>();
            bool isPickable = !isUsable && (targetedItem.GetComponent<Item>() || targetedItem.GetComponent<Item>());
            float requiredDistance = (isUsable ? targetedItem.GetComponent<UsableTarget>().UseDistance : (isPickable ? itemPickupDistance : float.MaxValue));
            float distance = Vector3.Distance(transform.position, targetedItem.transform.position);
            distanceCondition = distance <= requiredDistance;
            HighlightItem highlight = targetedItem.GetComponent<HighlightItem>();
            if (highlight && distanceCondition)
            {
                if(!highlight.outlineOn)
                {
                    highlight.OutlineOn();
                }
            }
        }
        else
        {
            return;
        }

        if (Input.GetKeyDown(USE_KEY_CODE))
        {
            handleUse(distanceCondition);
        }
        else if (Input.GetKeyUp(USE_KEY_CODE))
        {
            handleDeactivate();
        }
        else if (Input.GetKey(USE_KEY_CODE) && targetedItem.GetComponent<ConstantUsableTarget>() && distanceCondition)
        {
            targetedItem.GetComponent<ConstantUsableTarget>().Use();
        }
    }

    private void handleUse(bool distanceCondition)
    {
        if (targetedItem.GetComponent<DraggableItem>() && distanceCondition)
        {
            pickedUpItem = targetedItem;
            Debug.Log(pickedUpItem);
            Debug.Log(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().ChangeOwner(photonView.owner.ID);
            pickedUpItem.GetComponent<DraggableItem>().AttachToPlayer(photonView.viewID);
        }

        else if (targetedItem.GetComponent<Item>() && distanceCondition)
        {
            GetComponentInParent<PlayerInventory>().AddItem(targetedItem.GetComponent<Item>());
            targetedItem.GetComponent<Collider>().enabled = false;
            targetedItem.GetComponent<HighlightItem>().OutlineOff();
            Destroy(targetedItem.GetComponent<Item>());
            targetedItem = null;
        }

        else if (targetedItem.GetComponent<UsableTarget>() && distanceCondition)
        {
            targetedItem.GetComponent<UsableTarget>().Use();
        }
    }

    void handleDeactivate()
    {
         if (pickedUpItem)
        {
            pickedUpItem.GetComponent<DraggableItem>().DetachFromPlayer();
            pickedUpItem = null;
        }
    }

}
