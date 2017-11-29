using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    public List<Item> items;
    public List<Image> itemImages;

	public void AddItem(Item item) {
        items.Add(item);
        itemImages[items.IndexOf(item)].sprite = item.itemImage;
        itemImages[items.IndexOf(item)].enabled = true;
        item.transform.parent = transform;
        item.GetComponent<MeshRenderer>().enabled = false;
	}

    public void RemoveItem(Item item)
    {
        itemImages[items.IndexOf(item)].sprite = null;
        itemImages[items.IndexOf(item)].enabled = false;
        items.Remove(item);
    }

}
