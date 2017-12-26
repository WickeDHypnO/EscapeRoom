using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerModel : MonoBehaviour {

    public GameObject model;

	void Start () {
		if(GetComponent<PhotonView>().isMine)
        {
            model.SetActive(false);
        }
	}
	
}
