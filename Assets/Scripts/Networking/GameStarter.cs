using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : Photon.PunBehaviour {

    public GameObject playerPrefab;
    public List<Transform> playerStarts;

    void OnEnable () {
        if(PhotonNetwork.isMasterClient)
        StartGame();
        if(FindObjectOfType<LoadingScreenCanvas>())
        {
            FindObjectOfType<LoadingScreenCanvas>().FinishLoading();
        }
        else
        {
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        }
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
