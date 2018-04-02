using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWithHiddenDoors : MonoBehaviour {

    public GameObject[] TilesToMoveLeft;
    public GameObject[] TilesToMoveRight;
    public float OpeningTime = 3.0f;
    public float OpeningDistance = 1.0f;
    public float ZOffset = 0.3f;
    public bool Opened = false;
    private float elapsedTime;
    private bool moving;
    private bool close;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            float time = elapsedTime + Time.deltaTime;
            float distance = Time.deltaTime * OpeningDistance / OpeningTime;
            if (time >= OpeningTime)
            {
                distance = (OpeningTime - elapsedTime) * OpeningDistance / OpeningTime;
                moving = false;
                Opened = !Opened;
                if (!Opened) pullDoors(false);
            }
            foreach (GameObject tile in TilesToMoveLeft)
            {
                moveTileOnX(tile, distance, -1.0f);
            }
            foreach (GameObject tile in TilesToMoveRight)
            {
                moveTileOnX(tile, distance, 1.0f);
            }
            elapsedTime = time;
        }
	}

    public void Open()
    {
        if (moving || Opened) return;
        moving = true;
        close = false;
        elapsedTime = 0.0f;
        pullDoors(true);
    }

    public void Close()
    {
        if (moving || !Opened) return;
        moving = true;
        close = true;
        elapsedTime = 0.0f;
    }

    /*
    *   direction:
    *   -1 -> lewo
    *   1 -> prawo
    */
    private void moveTileOnX(GameObject tile, float distance, float direction)
    {
        Vector3 currentPos = tile.transform.position;
        float x = currentPos.x + distance * (close ? -1.0f * direction : 1.0f * direction);
        tile.transform.position = new Vector3(x, currentPos.y, currentPos.z);
    }

    /*
    *   direction:
    *   -1 -> do kamery
    *   1 -> od kamery
    */
    private void moveTileOnZ(GameObject tile, float distance, float direction)
    {
        Vector3 currentPos = tile.transform.position;
        float z = currentPos.z + distance * direction;
        tile.transform.position = new Vector3(currentPos.x, currentPos.y, z);
    }

    private void pullDoors(bool open)
    {
        float direction = (open ? 1.0f : -1.0f);
        foreach (GameObject tile in TilesToMoveLeft)
        {
            moveTileOnZ(tile, ZOffset, direction);
        }
        foreach (GameObject tile in TilesToMoveRight)
        {
            moveTileOnZ(tile, ZOffset, direction);
        }
    }
}
