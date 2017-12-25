using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckRotation : MonoBehaviour {

    public Transform neck;

    void Update()
    {
        if (transform.eulerAngles.x < 45 || transform.eulerAngles.x > 315)
        {
            neck.rotation = transform.rotation;
        }
    }
}
