using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableTargeter : MonoBehaviour {

    public LayerMask raycastMask;
    RaycastHit hitInfo;
    GameObject targetedItem;
	
	void Update () {
		if(Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f, raycastMask))
        {
            if (targetedItem != hitInfo.collider.gameObject)
            {
                targetedItem = hitInfo.collider.gameObject;               
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && targetedItem.GetComponent<Item>())
        {
            GetComponentInParent<PlayerInventory>().AddItem(targetedItem.GetComponent<Item>());
            targetedItem = null;
        }
    }
}
