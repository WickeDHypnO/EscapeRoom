using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : Photon.PunBehaviour {

	int count = 0;
	private void OnTriggerEnter (Collider other) {
		if (other.GetComponent<RigidbodyFirstPersonController> ()) {
			count++;
		}
		if (count == 2) {
			LoadRoom ();
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.GetComponent<RigidbodyFirstPersonController> ()) {
			count--;
		}
	}

	public void LoadRoom () {
		photonView.RPC ("RpcStartLoading", PhotonTargets.All, null);
	}

	[PunRPC]
	public void RpcNextLevel () {
		StartCoroutine (LoadRoomCor ());
	}
	public IEnumerator LoadRoomCor () {
		FindObjectOfType<LoadingScreenCanvas> ().StartLoading ();
		yield return new WaitForSeconds (1f);
		if (PhotonNetwork.isMasterClient)
			PhotonNetwork.LoadLevel (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}