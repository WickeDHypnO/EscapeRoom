using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugSpawner : MonoBehaviour {

    public GameObject player;

	void Start () {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("Offline");
        PhotonNetwork.Instantiate(player.name, transform.position, Quaternion.identity, 0);
	}
}
