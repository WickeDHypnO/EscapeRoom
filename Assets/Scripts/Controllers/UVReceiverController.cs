using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVReceiverController : MonoBehaviour {

    [SerializeField] private GameObject[] m_ObjectsToShow;
    [SerializeField] private UVLightColor[] m_TriggeringColors;

    public int[] m_LightsOnReceiver;
    private bool m_isOn;

	void Start ()
    {
        m_LightsOnReceiver = new int[(int)UVLightColor.END];
	}

    private bool isTriggered()
    {
        for (int i = 0; i < m_TriggeringColors.Length; i++)
        {
            if (m_LightsOnReceiver[(int)m_TriggeringColors[i]] <= 0)
                return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        UVLightController uvController = other.gameObject.GetComponent<UVLightController>();
        if(uvController)
        {
            m_LightsOnReceiver[(int)uvController.GetLightColor()]++;
            if(isTriggered() && !m_isOn)
            {
                ActivateObjects();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UVLightController uvController = other.gameObject.GetComponent<UVLightController>();
        if (uvController)
        {
            m_LightsOnReceiver[(int)uvController.GetLightColor()]--;
            if (!isTriggered() && m_isOn)
            {
                DeactivateObjects();
            }
        }
    }

    private void ActivateObjects()
    {
        foreach (GameObject gameobject in m_ObjectsToShow)
        {
            gameobject.SetActive(true);
        }
        m_isOn = true;
    }

    private void DeactivateObjects()
    {
        foreach (GameObject gameobject in m_ObjectsToShow)
        {
            gameobject.SetActive(false);
        }
        m_isOn = false;
    }
}
