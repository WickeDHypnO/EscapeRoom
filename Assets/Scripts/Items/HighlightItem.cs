using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightItem : MonoBehaviour {
    public GameObject outline;
    public bool outlineOn;

    public void OutlineOn()
    {
        outline.SetActive(true);
        outlineOn = true;
    }

    public void OutlineOff()
    {
        outline.SetActive(false);
        outlineOn = false;
    }
}
