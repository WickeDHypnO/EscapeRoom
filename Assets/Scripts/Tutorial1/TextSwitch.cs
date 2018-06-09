using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSwitch : MonoBehaviour {

    public GameObject text;

    private void OnTriggerEnter(Collider other)
    {
        text.SetActive(true);
    }
}
