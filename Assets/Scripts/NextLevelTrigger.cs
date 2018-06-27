using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : Photon.PunBehaviour, IPunObservable {

	public int count = 0;
	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player" && other.gameObject.GetPhotonView().owner == PhotonNetwork.player) {
			count++;
            photonView.RPC("RPCSynchronizeCount", PhotonTargets.All, count);
        }
		if (count == 2) {
			LoadRoom ();
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			count--;
            photonView.RPC("RPCSynchronizeCount", PhotonTargets.All, count);
        }
	}

    [PunRPC]
    public void RPCSynchronizeCount(int _count)
    {
        count = _count;
    }

	public void LoadRoom () {
		photonView.RPC ("RpcNextLevel", PhotonTargets.All, null);
	}

	[PunRPC]
	public void RpcNextLevel () {
		StartCoroutine (LoadRoomCor ());
	}
	public IEnumerator LoadRoomCor () {
		FindObjectOfType<LoadingScreenCanvas> ().StartLoading ();
		yield return new WaitForSeconds (1f);
		if (PhotonNetwork.isMasterClient) {
			if (SceneManager.GetSceneByBuildIndex (SceneManager.GetActiveScene ().buildIndex + 1) != null) {
				PhotonNetwork.LoadLevel (SceneManager.GetActiveScene ().buildIndex + 1);
			}
			else
			{
				PhotonNetwork.LoadLevel (0);
			}
		}
	}

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (count);
		} else {
			count = (int) stream.ReceiveNext ();
		}
	}

	private void Update() {
		#if DEBUG_VERSION
		if(Input.GetKeyDown(KeyCode.Alpha9))
		{
			LoadRoom();
		}
		#endif
	}
}