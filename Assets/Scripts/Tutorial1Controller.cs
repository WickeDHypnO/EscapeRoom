using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Controller : MonoBehaviour {

    public int platesPressed = 0;
    public DoorController door;
    public GameObject blockingVolume;
    public bool leverDown = false;

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

	public void platePressed()
    {
        platesPressed++;
        if (platesPressed == 2)
        {
            door.Open();
        }
    }
	public void leverSetDown()
    {
        leverDown = true;
        blockingVolume.SetActive(false);
    }
    public void plateReleased()
    {
        platesPressed--;
        if(!leverDown)
            door.Close();
    }
}
