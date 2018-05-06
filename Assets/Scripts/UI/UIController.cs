using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Photon.PunBehaviour
{
    public GameObject lobby;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject startGameButton;
    public List<Text> playerNames;
    public ConnectionManager connectionManager;
    public Text roomNumber;

    List<string> playerList;
    void Start()
    {
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
    }
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
        if (!PhotonNetwork.isMasterClient)
        {
            startGameButton.SetActive(false);
        }
        PhotonNetwork.automaticallySyncScene = true;
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        StartCoroutine(GetPlayerListDelayed());
    }


    public void LoadRoom()
    {
        StartCoroutine(LoadRoomCor());
    }
    public IEnumerator LoadRoomCor()
    {
        FindObjectOfType<LoadingScreenCanvas>().StartLoading();
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel(int.Parse(roomNumber.text));
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
