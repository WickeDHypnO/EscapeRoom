using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : Photon.PunBehaviour, IPunObservable {

	int count = 0;
	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			count++;
		}
		if (count == 2) {
			LoadRoom ();
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			count--;
		}
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
}