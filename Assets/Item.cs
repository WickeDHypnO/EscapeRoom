using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Photon.PunBehaviour {
    public Sprite itemImage;
    public bool flashlight = false;

    public void HideItem(int viewID)
    {
        photonView.RPC("RpcHideItem", PhotonTargets.All, viewID);
    }

    [PunRPC]
    void RpcHideItem(int viewID)
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        GetComponent<HighlightItem>().OutlineOff();
        transform.parent = PhotonView.Find(viewID).transform;
        if(flashlight)
        transform.parent.GetComponentInChildren<FlashlightController>().flashlightEnabled = true;
        transform.localPosition = Vector3.zero;
    }
}
