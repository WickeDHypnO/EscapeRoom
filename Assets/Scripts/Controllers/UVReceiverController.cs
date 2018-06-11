using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UVReceiverController : MonoBehaviour {

    [SerializeField] private UnityEvent m_onActivation;
    [SerializeField] private UnityEvent m_onDeactivation;
    [SerializeField] private UVLightColor[] m_TriggeringColors;

    private int[] m_LightsOnReceiver;
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
        m_onActivation.Invoke();
        m_isOn = true;
    }

    private void DeactivateObjects()
    {
        m_onDeactivation.Invoke();
        m_isOn = false;
    }
}
