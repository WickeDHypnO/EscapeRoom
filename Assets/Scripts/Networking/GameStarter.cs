using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : Photon.PunBehaviour {

    public GameObject playerPrefab;
    public List<Transform> playerStarts;

    void OnEnable () {
        if(PhotonNetwork.isMasterClient)
        StartGame();
	}

    public void StartGame()
    {
        foreach (PhotonPlayer pp in PhotonNetwork.playerList)
        {
            photonView.RPC("RpcCreatePlayer", pp);
        }
    }

    [PunRPC]
    void RpcCreatePlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, PhotonNetwork.isMasterClient ? playerStarts[0].position : playerStarts[1].position, Quaternion.identity, 0);
        gameObject.SetActive(false);
    }

}
