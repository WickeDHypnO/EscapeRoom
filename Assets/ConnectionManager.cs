using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : Photon.PunBehaviour
{

    public InputField serverName;
    public Text errorText;
    public Text serverNameText;
    public Button connectButton;
    public Button startButton;
    public GameObject playerPrefab;
    public GameObject connectionUI;
    public Text playerList;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        connectionUI.SetActive(true);
    }

    public void Connect()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        ro.IsOpen = true;
        ro.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(serverName.text, ro, TypedLobby.Default);
        PhotonNetwork.player.NickName = "Dupa";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.player.NickName = "DupaMaster";
        serverNameText.text = "Server Name: " + PhotonNetwork.room.Name;
        serverName.gameObject.SetActive(false);
        connectButton.gameObject.SetActive(false);
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
        PhotonNetwork.Instantiate(playerPrefab.name, PhotonNetwork.isMasterClient ? Vector3.zero : Vector3.forward * 2, Quaternion.identity, 0);
        gameObject.SetActive(false);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        playerList.text = "";
        foreach (PhotonPlayer pp in PhotonNetwork.playerList)
        {
            playerList.text += pp.NickName + "\n";
        }
        if (PhotonNetwork.isMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }
    }
}
