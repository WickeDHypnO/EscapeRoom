using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeObserver : MonoBehaviour {

    [SerializeField] private Transform m_targetToObserve;

    [SerializeField] private float m_minimalHeight;

    [SerializeField] private GameObject m_OutlineHolder;
    private UsableItemPlaceholder m_ItemPlaceholder;

    private bool m_lastState;

    private void Start()
    {
        m_lastState = m_targetToObserve.position.y >= m_minimalHeight;
        m_ItemPlaceholder = this.GetComponent<UsableItemPlaceholder>();
    }

    // Update is called once per frame
    void Update ()
    {
		bool currentState = m_targetToObserve.position.y >= m_minimalHeight;

        if (currentState != m_lastState)
        {
            m_lastState = currentState;

            m_OutlineHolder.SetActive(currentState);
            if(m_ItemPlaceholder)
            {
                m_ItemPlaceholder.m_canUse = currentState;
            }
        }
    }
}
