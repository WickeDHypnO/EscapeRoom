using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWithHiddenDoors : MonoBehaviour {

    public GameObject[] TilesToMoveLeft;
    public GameObject[] TilesToMoveRight;
    public float OpeningTime = 3.0f;
    public float OpeningDistance = 1.0f;
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
            }
            foreach (GameObject tile in TilesToMoveLeft)
            {
                Vector3 currentPos = tile.transform.position;
                float x = currentPos.x + distance * (close ? 1.0f : -1.0f);
                tile.transform.position = new Vector3(x, currentPos.y, currentPos.z);
            }
            foreach (GameObject tile in TilesToMoveRight)
            {
                Vector3 currentPos = tile.transform.position;
                float x = currentPos.x + distance * (close ? -1.0f : 1.0f);
                tile.transform.position = new Vector3(x, currentPos.y, currentPos.z);
            }
            elapsedTime = time;
        }
	}

    public void Open()
    {
        if (moving) return;
        moving = true;
        close = false;
        elapsedTime = 0.0f;
    }

    public void Close()
    {
        if (moving) return;
        moving = true;
        close = true;
        elapsedTime = 0.0f;
    }
}
