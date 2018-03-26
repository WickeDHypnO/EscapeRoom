using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateController : MonoBehaviour
{

    public float neededWeight;
    public bool pressed = false;
    float currentWeight;
    public GameObject plate;
    public float pressedHeightDelta;
    float defaultHeight;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

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
            onPressed.Invoke();
        }
        else
        {
            pressed = false;
        }
        ChangeVisualState(!pressed);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<Rigidbody>())
        {
            currentWeight -= col.GetComponent<Rigidbody>().mass;
        }
        if (currentWeight >= neededWeight)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
            onReleased.Invoke();
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

