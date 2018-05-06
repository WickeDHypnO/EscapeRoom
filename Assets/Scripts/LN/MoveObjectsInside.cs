using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectsInside : MonoBehaviour {

    private HashSet<GameObject> objectsInside;

    // Use this for initialization
    void Start ()
    {
        objectsInside = new HashSet<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {}

    void OnTriggerEnter(Collider other)
    {
        objectsInside.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        objectsInside.Remove(other.gameObject);
    }

    public void MoveObjectsInsideBy(float yDist)
    {
        foreach (GameObject obj in objectsInside)
        {
            Vector3 objPos = obj.transform.position;
            float y = objPos.y + yDist;
            obj.transform.position = new Vector3(objPos.x, y, objPos.z);
        }
    }
}
