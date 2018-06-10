using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumController : MonoBehaviour {

    [SerializeField] private GameObject m_targetTransform;

    [SerializeField] private float m_FillingTime;

    private Vector3 m_originalPos;
    private Vector3 m_originalScale;

    private Vector3 m_targetPos;
    private Vector3 m_targetScale;

    private 

	void Start ()
    {
        m_originalPos = this.transform.position;
        m_originalScale = this.transform.localScale;

        m_targetPos = m_targetTransform.transform.position;
        m_targetScale = m_targetTransform.transform.localScale;

        Destroy(m_targetTransform);

        StartCoroutine(FillWater());
    }

    IEnumerator FillWater()
    {
        float timer = 0.0f;
        while (timer < 1.0f)
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f / m_FillingTime;

            transform.position = Vector3.Lerp(m_originalPos, m_targetPos, timer);
            transform.localScale = Vector3.Lerp(m_originalScale, m_targetScale, timer);
        }
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, defaultRotation + openDegrees, transform.localEulerAngles.z);
    }

}
