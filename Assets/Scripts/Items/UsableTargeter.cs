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
    private PlayerInventory inventory;
    private string itemToUse = null;

    void Start()
    {
        inventory = GetComponentInParent<PlayerInventory>();
    }

    void LateUpdate()
    {
        bool distanceCondition = false;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 50f, raycastMask))
        {
            if (targetedItem != hitInfo.collider.gameObject)
            {
                checkAndCleanItem();
                if (targetedItem && targetedItem.GetComponent<HighlightItem>())
                {
                    targetedItem.GetComponent<HighlightItem>().OutlineOff();
                }
                targetedItem = hitInfo.collider.gameObject;
            }
            bool isUsable = targetedItem.GetComponent<UsableTarget>();
            bool isPickable = !isUsable && (targetedItem.GetComponent<Item>() || targetedItem.GetComponent<Item>() || targetedItem.GetComponent<DraggableItem>());
            float requiredDistance = (isUsable ? targetedItem.GetComponent<UsableTarget>().UseDistance : (isPickable ? itemPickupDistance : float.MaxValue));
            float distance = Vector3.Distance(transform.position, targetedItem.transform.position);
            distanceCondition = distance <= requiredDistance;
            if (distanceCondition)
            {
                if (isUsable && string.IsNullOrEmpty(itemToUse))
                {
                    UsableTarget ut = targetedItem.GetComponent<UsableTarget>();
                    if (ut.UsesItems)
                    {
                        itemToUse = checkItems(ut);
                        if (!string.IsNullOrEmpty(itemToUse))
                        {
                            ut.SetOutlineColour(HighlightColours.ITEM_USE_COLOUR);
                            inventory.HighlightItem(itemToUse, true);
                        }
                    }
                }
                HighlightItem highlight = targetedItem.GetComponent<HighlightItem>();
                if (highlight != null)
                {
                    highlight.OutlineOn();
                }
            }
        }
        else
        {
            checkAndCleanItem();
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
            usableTargetUse();
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
            inventory.AddItem(targetedItem.GetComponent<Item>());
            targetedItem.GetComponent<Collider>().enabled = false;
            targetedItem.GetComponent<HighlightItem>().OutlineOff();
            Destroy(targetedItem.GetComponent<Item>());
            targetedItem = null;
        }
        else if (targetedItem.GetComponent<UsableTarget>() && distanceCondition)
        {
            usableTargetUse();
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

    private string checkItems(UsableTarget targetObject)
    {
        string id;
        foreach (Item item in inventory.items)
        {
            id = item.ItemId;
            if (targetObject.CheckItemOnTrace(id))
            {
                return id;
            }
        }
        return null;
    }

    private bool tryUsingItem(UsableTarget target)
    {
        if (target.UseItem(itemToUse))
        {
            bool itemRemoved = !inventory.HandleItemUse(itemToUse);
            cleanItemToUse(!itemRemoved);
            return true;
        }
        return false;
    }

    private void cleanItemToUse(bool disableItemHighlight)
    {
        if (disableItemHighlight)
        {
            inventory.HighlightItem(itemToUse, false);
        }
        targetedItem.GetComponent<UsableTarget>().RevertOutlineColour();
        itemToUse = null;
    }

    private void checkAndCleanItem()
    {
        if (!string.IsNullOrEmpty(itemToUse))
        {
            cleanItemToUse(true);
        }
    }

    private void usableTargetUse()
    {
        UsableTarget ut = targetedItem.GetComponent<UsableTarget>();
        if (string.IsNullOrEmpty(itemToUse) || !tryUsingItem(ut))
        {
            ut.Use();
        }
    }
}
