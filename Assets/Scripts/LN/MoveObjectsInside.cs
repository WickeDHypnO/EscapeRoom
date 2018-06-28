using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectsInside : MonoBehaviour {

    private HashSet<GameObject> objectsInside;
    //private HashSet<int> objectsInsideViewIDs;
    //private Transform viewTransform;

    // Use this for initialization
    void Start ()
    {
        objectsInside = new HashSet<GameObject>();
        /*objectsInsideViewIDs = new HashSet<int>();
        viewTransform = GetComponent<PhotonView>().transform;*/
    }
	
	// Update is called once per frame
	void Update () {}

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        /*if (obj.transform.parent == null)
        {
            obj.transform.parent = transform;
        }*/
        objectsInside.Add(obj);
    }

    void OnTriggerStay(Collider other)
    {
        /*GameObject obj = other.gameObject;
        if (!objectsInside.Contains(obj) && (obj.transform.parent == null))
        {
            obj.transform.SetParent(transform, true);
            objectsInside.Add(obj);
        }*/

        /*PhotonView view = other.gameObject.GetPhotonView();
        int viewId = view.viewID;
        Transform objTransform = view.transform;
        if (!objectsInsideViewIDs.Contains(viewId) && (objTransform.parent == null))
        {
            objTransform.SetParent(viewTransform, true);
            objectsInsideViewIDs.Add(viewId);
        }*/
    }

    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        /*if (obj.transform.parent == transform)
        {
            obj.transform.parent = null;
        }*/
        objectsInside.Remove(obj);

        /*PhotonView view = other.gameObject.GetPhotonView();
        int viewId = view.viewID;
        Transform objTransform = view.transform;
        if (objTransform.parent == viewTransform)
        {
            objTransform.SetParent(null);
        }
        objectsInsideViewIDs.Remove(viewId);*/
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
