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
    private bool puzzleElementMatching = false;

    void Start()
    {
        inventory = GetComponentInParent<PlayerInventory>();
    }

    void LateUpdate()
    {
        bool distanceCondition = false;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 50f, raycastMask, QueryTriggerInteraction.Ignore))
        {
            if (targetedItem != hitInfo.collider.gameObject)
            {
                checkAndCleanItem();
                if (targetedItem && targetedItem.GetComponent<HighlightItem>())
                {
                    if (targetedItem.GetComponent<PuzzleElementPlaceholder>())
                    {
                        targetedItem.GetComponent<PuzzleElementPlaceholder>().SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
                    }
                    else
                    {
                        targetedItem.GetComponent<HighlightItem>().OutlineOff();
                    }
                }
                targetedItem = hitInfo.collider.gameObject;
            }
            bool isUsable = targetedItem.GetComponent<UsableTarget>();
            bool isDraggable = !isUsable && (targetedItem.GetComponent<DraggableItem>());
            bool isPickable = !isUsable && (targetedItem.GetComponent<Item>() || targetedItem.GetComponent<Item>());
            bool isPuzzle = !isUsable && !isPickable && targetedItem.GetComponent<PuzzleElementPlaceholder>();
            float requiredDistance = (isUsable ? targetedItem.GetComponent<UsableTarget>().UseDistance : (isDraggable ? targetedItem.GetComponent<DraggableItem>().UseDistance : (isPickable || isPuzzle ? itemPickupDistance : float.MaxValue)));
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
                if (isPuzzle && (pickedUpItem != null))
                {
                    DraggableItem di = pickedUpItem.GetComponent<DraggableItem>();
                    PuzzleElementPlaceholder pep = targetedItem.GetComponent<PuzzleElementPlaceholder>();
                    if (di.IsPuzzleElement)
                    {
                        puzzleElementMatching = pep.CheckElementOnTrace(di.PuzzleElementId);
                    }
                    pep.SetOutlineColour((puzzleElementMatching ? HighlightColours.ITEM_USE_COLOUR : HighlightColours.WRONG_ELEMENT_COLOUR));
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
        else if (Input.GetKeyUp(USE_KEY_CODE) && (pickedUpItem != null))
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
        bool puzzleMatched = false;
        PuzzleElementPlaceholder pep = targetedItem.GetComponent<PuzzleElementPlaceholder>();
        if (pep != null)
        {
            pep.SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
            puzzleMatched = puzzleElementMatching;
        }
        pickedUpItem.GetComponent<DraggableItem>().DetachFromPlayer();
        if (puzzleMatched)
        {
            pep.PlaceElement(pickedUpItem);
        }
        pickedUpItem = null;
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
        bool isReadable = targetedItem.GetComponent<TextViewer>();
        
        if (isReadable)
        {
            targetedItem.GetComponent<TextViewer>().SetPlayerRigidbody(GetComponentInParent<RigidbodyFirstPersonController>());
        }
        if (string.IsNullOrEmpty(itemToUse) || !tryUsingItem(ut))
        {
            ut.Use();
        }
    }
}
