using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : Photon.PunBehaviour
{
    public Text infoText;
    public GameObject connectionUI;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        infoText.gameObject.SetActive(false);
        connectionUI.SetActive(true);
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        infoText.text = cause.ToString();
    }

    public List<string> GetPlayerList()
    {
        List<string> players = new List<string>();
        foreach(PhotonPlayer pp in PhotonNetwork.playerList)
        {
            players.Add(pp.NickName);
        }
        return players;
    }

    public IEnumerator ConnectOrCreate()
    {
        yield return PhotonNetwork.JoinRandomRoom();
        if(!PhotonNetwork.inRoom)
        {
            CreateRoom();
        }
    }

    public void CreateRoom()
    {
        Debug.Log("Creating");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        ro.IsOpen = true;
        ro.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("asd" + Random.Range(0, 20), ro, TypedLobby.Default);
        PhotonNetwork.player.NickName = "Master";
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.isNonMasterClientInRoom)
        PhotonNetwork.player.NickName = "Slave";
    }
}
