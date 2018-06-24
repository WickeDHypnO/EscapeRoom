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

    /*void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.transform.parent == null)
        {
            obj.transform.parent = transform;
        }
        objectsInside.Add(other.gameObject);
    }*/

    void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        if (!objectsInside.Contains(obj) && (obj.transform.parent == null))
        {
            obj.transform.parent = transform;
            objectsInside.Add(obj);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.transform.parent == transform)
        {
            obj.transform.parent = null;
        }
        objectsInside.Remove(obj);
    }

    public void MoveObjectsInsideBy(float yDist)
    {
        /*foreach (GameObject obj in objectsInside)
        {
            Vector3 objPos = obj.transform.position;
            float y = objPos.y + yDist;
            obj.transform.position = new Vector3(objPos.x, y, objPos.z);
        }*/
    }
}
