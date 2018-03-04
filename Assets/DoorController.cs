using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public float openDegrees;
    public float openTime;
    float defaultRotation;

    void Start()
    {
        defaultRotation = transform.eulerAngles.y;
    }

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenDoor());
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()
    {
        float timer = 0f;
        while (!transform.localEulerAngles.y.AlmostEquals(defaultRotation + openDegrees, 0.1f))
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f / openTime;
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z),
                new Vector3(transform.localEulerAngles.x, defaultRotation + openDegrees, transform.localEulerAngles.z),
                timer));
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, defaultRotation + openDegrees, transform.localEulerAngles.z);
    }

    IEnumerator CloseDoor()
    {
        float timer = 0f;
        while (!transform.localEulerAngles.y.AlmostEquals(defaultRotation, 0.1f))
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f / openTime;
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z),
                new Vector3(transform.localEulerAngles.x, defaultRotation, transform.localEulerAngles.z),
                timer));
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, defaultRotation, transform.localEulerAngles.z);
    }
}
