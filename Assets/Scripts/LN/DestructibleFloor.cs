using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleFloor : MonoBehaviour {

    public GameObject[] FloorTilesToRemove;
    private bool tilesDestroyed;

	// Use this for initialization
	void Start () {
        tilesDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {}

    public void DestroyTiles()
    {
        if (tilesDestroyed) return;
        foreach (GameObject obj in FloorTilesToRemove)
        {
            GameObject.Destroy(obj);
        }
        tilesDestroyed = true;
    }

    public bool areTilesDestroyed()
    {
        return tilesDestroyed;
    }
}
