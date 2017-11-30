using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Photon.PunBehaviour {
    public Sprite itemImage;

    public void HideItem(int viewID)
    {
        photonView.RPC("RpcHideItem", PhotonTargets.All, viewID);
    }

    [PunRPC]
    void RpcHideItem(int viewID)
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<HighlightItem>().OutlineOff();
        transform.parent = PhotonView.Find(viewID).transform;
        transform.localPosition = Vector3.zero;
    }
}
