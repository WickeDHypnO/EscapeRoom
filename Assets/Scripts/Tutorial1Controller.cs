using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Controller : MonoBehaviour {

    public DoorController door;
    public GameObject blockingVolume;
    private bool leverDown = false;
    private bool[] platesPressed = { false, false };

    void Start()
    {
        if(!FindObjectOfType<LoadingScreenCanvas>())
        {
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        }
        else
        {
            FindObjectOfType<LoadingScreenCanvas>().FinishLoading();
        }
        
    }

	public void PlatePressed(int id)
    {
        platesPressed[id] = true;
        if (platesPressed[0] && platesPressed[1])
        {
            door.Open();
        }
    }

	public void LeverSetDown()
    {
        leverDown = true;
        blockingVolume.SetActive(false);
    }

    public void PlateReleased(int id)
    {
        platesPressed[id] = false;
        if(!leverDown)
            door.Close();
    }
}
