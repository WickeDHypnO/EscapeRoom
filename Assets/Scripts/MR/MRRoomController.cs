using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MRRoomController : MonoBehaviour {

    public DoorController hiddenDoor;
    public DoorController bookDoor;
    public DoorController studyDoor;
    public DoorController galleryDoor;
    private bool[] gallerySwitchesDown = { false, false, false, false, false, false };
    private bool[] platesPressed = { false, false, false, false, false, false, false };

    // Use this for initialization
    void Start () {
        if (!FindObjectOfType<LoadingScreenCanvas>())
        {
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        }
        else
        {
            FindObjectOfType<LoadingScreenCanvas>().FinishLoading();
        }

    }

    public void GallerySwitchDown(int id)
    {
        gallerySwitchesDown[id] = true;
        CheckSwitches();
    }

    public void GallerySwitchUp(int id)
    {
        gallerySwitchesDown[id] = false;
        CheckSwitches();
    }

    private void CheckSwitches()
    {
        if (!gallerySwitchesDown[0] && gallerySwitchesDown[1] && !gallerySwitchesDown[2] && gallerySwitchesDown[3] && gallerySwitchesDown[4] && !gallerySwitchesDown[5])
        {
            bookDoor.Open();
            galleryDoor.Close();
        }
        else if (gallerySwitchesDown[0] && !gallerySwitchesDown[1] && gallerySwitchesDown[2] && !gallerySwitchesDown[3] && !gallerySwitchesDown[4] && gallerySwitchesDown[5])
        {
            bookDoor.Close();
            galleryDoor.Open();
        }
        else
        {
            bookDoor.Close();
            galleryDoor.Close();
        }
    }

    public void GalleryPlatePressed()
    {
        hiddenDoor.Open();
    }

    public void GalleryPlateReleased()
    {
        hiddenDoor.Close();
    }

    public void PlatePressed(int id)
    {
        platesPressed[id] = true;
        if (!platesPressed[0] && platesPressed[1] && !platesPressed[2] && !platesPressed[3] && platesPressed[4] && !platesPressed[5] && platesPressed[6])
        {
            End();
        }
    }

    public void PlateReleased(int id)
    {
        platesPressed[id] = false;
        if (!platesPressed[0] && platesPressed[1] && !platesPressed[2] && !platesPressed[3] && platesPressed[4] && !platesPressed[5] && platesPressed[6])
        {
            End();
        }
    }

    private void End()
    {

    }
}
