using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRStudyDoorController : UsableDoorController
{
    public const string KEY_ITEM_ID = "StudyKey";

    public override bool CheckItemOnTrace(string itemId)
    {
        return (Locked && (itemId == KEY_ITEM_ID));
    }

    public override bool UseItem(string itemId)
    {
        if (Locked && (itemId == KEY_ITEM_ID))
        {
            Unlock();
            return true;
        }
        return false;
    }
}
