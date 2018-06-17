using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LNWardrobeDoorsController : UsableDoorController {
    public GameObject SecondDoor;
    public const string KEY_ITEM_ID = "WardrobeKey";

    public override bool CheckItemOnTrace (string itemId) {
        return (Locked && (itemId == KEY_ITEM_ID));
    }

    public override bool UseItem (string itemId) {
        if (Locked && (itemId == KEY_ITEM_ID)) {
            Unlock ();
            if (SecondDoor != null) {
                SecondDoor.GetComponent<LNWardrobeDoorsController> ().Unlock ();
            }
            return true;
        }
        return false;
    }

}