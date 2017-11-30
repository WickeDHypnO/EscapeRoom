using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem : Photon.PunBehaviour {

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
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(PhotonView.Find(viewID).transform);
    }

    [PunRPC]
    void RpcDetachFromPlayer()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
