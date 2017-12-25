using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem : Photon.PunBehaviour
{

    public bool freeMovement = true;

    public void ChangeOwner(int playerID)
    {
        photonView.TransferOwnership(playerID);
    }

    public void AttachToPlayer(int viewID)
    {
        photonView.RPC("RpcAttachToPlayer", PhotonTargets.All, viewID);
    }

    public void DetachFromPlayer()
    {
        photonView.RPC("RpcDetachFromPlayer", PhotonTargets.All);
    }

    [PunRPC]
    void RpcAttachToPlayer(int viewID)
    {
        if (freeMovement)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.SetParent(PhotonView.Find(viewID).transform);
        }
        else
        {
            GetComponent<ConfigurableJoint>().connectedBody = PhotonView.Find(viewID).GetComponent<Rigidbody>();
        }
    }

    [PunRPC]
    void RpcDetachFromPlayer()
    {

        if (freeMovement)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.SetParent(null);
        }
        else
        {
            GetComponent<ConfigurableJoint>().connectedBody = null;
        }
    }
}
