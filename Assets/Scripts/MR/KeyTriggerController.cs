using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTriggerController : MonoBehaviour {

    
    public DoorController studyDoor;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < other.gameObject.GetComponent<PlayerInventory>().items.Count; i++)
        {
            if (other.gameObject.GetComponent<PlayerInventory>().items[i].ItemId == "StudyKey")
            {
                other.gameObject.GetComponent<PlayerInventory>().items[i].HideItem(i);
                studyDoor.Open();
            }
        }

    }

// Use this for initialization
void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
