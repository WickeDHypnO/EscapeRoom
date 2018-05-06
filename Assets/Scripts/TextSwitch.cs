using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSwitch : MonoBehaviour {

    bool bTextOn = false;
    public GameObject text;

    public void changeTextState()
    {
        bTextOn = !bTextOn;
        text.SetActive(bTextOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        changeTextState();
    }
}
