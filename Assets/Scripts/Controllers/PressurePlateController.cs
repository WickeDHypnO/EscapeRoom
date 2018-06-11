using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}

public class PressurePlateController : MonoBehaviour
{

    public float neededWeight;
    public bool pressed = false;
    public int id;
    float currentWeight;
    public GameObject plate;
    public float pressedHeightDelta;
    float defaultHeight;
    public IntEvent onPressed;
    public IntEvent onReleased;
    public bool InvokeEvents = true;

    public void SetInvokeEvents(bool invokeEvents)
    {
        InvokeEvents = invokeEvents;
    }

    void Start()
    {
        defaultHeight = plate.transform.localPosition.y;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Rigidbody>())
        {
            currentWeight += col.GetComponent<Rigidbody>().mass;
        }
        if (currentWeight >= neededWeight)
        {
            pressed = true;
            if (InvokeEvents)
            {
                onPressed.Invoke(id);
            }
        }
        else
        {
            pressed = false;
        }
        ChangeVisualState(!pressed);
    }

    void OnTriggerExit(Collider col)
    {
        bool wasPressed = currentWeight >= neededWeight;
        if (col.GetComponent<Rigidbody>())
        {
            currentWeight -= col.GetComponent<Rigidbody>().mass;
        }
        if (currentWeight >= neededWeight)
        {
            pressed = true;
        }
        else if (wasPressed)
        {
            pressed = false;
            if (InvokeEvents)
            {
                onReleased.Invoke(id);
            }
        }
        ChangeVisualState(!pressed);
    }

    void ChangeVisualState(bool up)
    {
        StopAllCoroutines();
        StartCoroutine(Move(up));
    }

    IEnumerator Move(bool up)
    {
        bool moving = true;
        float timer = 0;
        if (!up)
        {
            timer = 1f;
        }
        while (moving)
        {
            yield return new WaitForSeconds(0.01f);
            if (up)
            {
                timer += 0.1f;
            }
            else
            {
                timer -= 0.1f;
            }
            plate.transform.localPosition = Vector3.Lerp(
                new Vector3(plate.transform.localPosition.x, defaultHeight - pressedHeightDelta, plate.transform.localPosition.z),
                new Vector3(plate.transform.localPosition.x, defaultHeight, plate.transform.localPosition.z),
                timer);
            if (up)
            {
                if (timer >= 1f)
                {
                    moving = false;
                }
            }
            else
            {
                if (timer <= 0f)
                {
                    moving = false;
                }
            }
        }
    }
}

