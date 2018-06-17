using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingTableController : UsableTarget {

    public List<GameObject> ladderParts = new List<GameObject> ();
    public GameObject LadderStep;

    public override bool CheckItemOnTrace (string itemId) {
        return (itemId == "Hammer") && (ladderParts.Count == 3);
    }
    public override void Use () {
        return;
    }
    public override bool UseItem (string itemId) {
        if ((itemId == "Hammer") && (ladderParts.Count == 3)) {
            foreach (GameObject part in ladderParts) {
                photonView.RPC ("RpcDestroyPart", PhotonNetwork.masterClient, part.GetPhotonView ().viewID);
            }
            ladderParts.Clear ();
            PhotonNetwork.Instantiate (LadderStep.name, this.transform.position + new Vector3 (0, 1, 0), Quaternion.identity, 0);
            return true;
        }
        return false;
    }

    public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        return;
    }

    public void ObjectReleased (GameObject obj) {
        if (obj.tag.Equals ("Ladder") && ladderParts.Contains (obj)) {
            obj.GetComponent<DraggableItem> ().enabled = false;
        }
    }

    [PunRPC]
    void RpcDestroyPart (int viewId) {
        PhotonNetwork.Destroy (PhotonView.Find (viewId).gameObject);
    }
}