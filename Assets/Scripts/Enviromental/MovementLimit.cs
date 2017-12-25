using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimit : MonoBehaviour
{

    public Vector3 movementLimit;
    Vector3 defaultPosition;
    public bool switchMovement = true;

    void Start()
    {
        defaultPosition = transform.localPosition;
    }

    void Update()
    {
        if (transform.localPosition.x - defaultPosition.x >= 0)
        {
            transform.localPosition = new Vector3(defaultPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.x - defaultPosition.x <= movementLimit.x)
        {
            transform.localPosition = new Vector3(defaultPosition.x + movementLimit.x, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y - defaultPosition.y >= 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.y - defaultPosition.y <= movementLimit.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosition.y + movementLimit.y, transform.localPosition.z);
        }
        if (transform.localPosition.z - defaultPosition.z >= 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultPosition.z);
        }
        else if (transform.localPosition.z - defaultPosition.z <= movementLimit.z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultPosition.z + movementLimit.z);
        }
    }
}
