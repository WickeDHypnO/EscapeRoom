using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableTargeter : MonoBehaviour
{

    public LayerMask raycastMask;
    public float itemPickupDistance = 2f;
    RaycastHit hitInfo;
    GameObject targetedItem;
    GameObject pickedUpItem;

    void FixedUpdate()
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

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<DraggableItem>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            pickedUpItem = targetedItem;
            pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pickedUpItem.transform.SetParent(transform);
        }

        if (Input.GetKeyUp(KeyCode.E) && pickedUpItem)
        {
            pickedUpItem.transform.SetParent(null);
            pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            pickedUpItem = null;
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<Item>() && Vector3.Distance(transform.position, targetedItem.transform.position) < itemPickupDistance)
        {
            GetComponentInParent<PlayerInventory>().AddItem(targetedItem.GetComponent<Item>());
            targetedItem.GetComponent<Collider>().enabled = false;
            targetedItem.GetComponent<HighlightItem>().OutlineOff();
            targetedItem = null;
        }
    }

}
