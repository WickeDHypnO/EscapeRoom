using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Photon.PunBehaviour {

    public List<Item> items;
    public List<Image> itemImages;
    private static readonly Vector4 ITEM_HIGHLIGHT_COLOUR = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

    public void AddItem(Item item) {
        items.Add(item);
        itemImages[items.IndexOf(item)].sprite = item.itemImage;
        itemImages[items.IndexOf(item)].enabled = true;
        item.HideItem(photonView.viewID);
	}

    public void RemoveItem(Item item)
    {
        int index = items.IndexOf(item);
        removeByIndex(index);
    }

    /*
     * Zwraca true jeśli przedmiot można dalej używać.
     */
    public bool HandleItemUse(string itemId)
    {
        int index = getIndexById(itemId);
        if (index < 0) return false;
        Item item = items[index];
        --item.UsesCount;
        if (item.UsesCount < 1)
        {
            removeByIndex(index);
            return false;
        }
        return true;
    }

    public void HighlightItem(string itemId, bool highlight)
    {
        int index = getIndexById(itemId);
        if (index < 0) return;
        Vector4 colour = (highlight ? ITEM_HIGHLIGHT_COLOUR : new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        itemImages[index].color = new Color(colour.x, colour.y, colour.z, colour.w);
    }

    private int getIndexById(string itemId)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].ItemId == itemId)
            {
                return i;
            }
        }
        return -1;
    }

    private void removeByIndex(int index)
    {
        itemImages[index].sprite = null;
        itemImages[index].enabled = false;
        itemImages.RemoveAt(index);
        items.RemoveAt(index);
    }
}
