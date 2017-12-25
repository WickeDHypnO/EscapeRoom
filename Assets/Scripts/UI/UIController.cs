﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Photon.PunBehaviour
{
    public GameObject lobby;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public List<Text> playerNames;
    public ConnectionManager connectionManager;
    public Text roomNumber;

    List<string> playerList;

    public void ExitGame()
    {
        if(PhotonNetwork.connected)
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void ConnectOrCreate()
    {
        StartCoroutine(EstablishState());
    }

    IEnumerator EstablishState()
    {
        yield return connectionManager.ConnectOrCreate();
        Debug.Log("TESTING");
    }

    public override void OnJoinedRoom()
    {
        lobby.SetActive(true);
        mainMenu.SetActive(false);
        StartCoroutine(GetPlayerListDelayed());
        PhotonNetwork.automaticallySyncScene = true;
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        StartCoroutine(GetPlayerListDelayed());
    }

    public void LoadRoom()
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel(int.Parse(roomNumber.text));
        //photonView.RPC("RpcLoadRoom", PhotonTargets.All, );
    }

    [PunRPC]
    void RpcLoadRoom(int room)
    {
        SceneManager.LoadScene(room);
    }

    IEnumerator GetPlayerListDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        playerList = connectionManager.GetPlayerList();
        for (int i = 0; i < playerList.Count; i++)
        {
            playerNames[i].text = playerList[i];
        }
    }
}