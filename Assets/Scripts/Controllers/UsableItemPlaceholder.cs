using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UsableItemPlaceholder : UsableTarget
{
    public GameObject m_actualGameObject;
    public UnityEvent m_eventToExecute;

    public string KEY_ITEM_ID;

    public override bool CheckItemOnTrace(string itemId)
    {
        return (itemId == KEY_ITEM_ID);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.isWriting)
        {
            stream.SendNext(opened);
            stream.SendNext(elapsedTime);
            stream.SendNext(moving);
        }
        else
        {
            opened = (bool)stream.ReceiveNext();
            elapsedTime = (float)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
        }*/
    }

    [PunRPC]
    private void RPCUse()
    {
        m_actualGameObject.SetActive(true);
        m_eventToExecute.Invoke();
        Destroy(this.gameObject);
    }

    public override void Use()
    {
        //photonView.RPC("RPCUseItem", PhotonTargets.All);
    }

    public override bool UseItem(string itemId)
    {
        if (itemId == KEY_ITEM_ID)
        {
            photonView.RPC("RPCUse", PhotonTargets.All);
            return true;
        }
        return false;
    }

    protected override void initialize()
    {
        SetOutlineColour(HighlightColours.INACTIVE_COLOUR);
    }
}
