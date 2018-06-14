using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumController : MonoBehaviour {

    [SerializeField] private GameObject m_waterObject;
    [SerializeField] private GameObject m_targetTransform;

    [SerializeField] private float m_FillingTime;

    [SerializeField]
    private GameObject m_realKey;

    [SerializeField]
    private GameObject m_fakeKey;

    private Vector3 m_originalPos;
    private Vector3 m_originalScale;

    private Vector3 m_targetPos;
    private Vector3 m_targetScale;

    private bool m_filled;

	void Start ()
    {
        m_originalPos = m_waterObject.transform.position;
        m_originalScale = m_waterObject.transform.localScale;

        m_targetPos = m_targetTransform.transform.position;
        m_targetScale = m_targetTransform.transform.localScale;

        Destroy(m_targetTransform);

        m_filled = false;
    }

    IEnumerator FillWater()
    {
        float timer = 0.0f;
        while (timer < 1.0f)
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f / m_FillingTime;

            m_waterObject.transform.position = Vector3.Lerp(m_originalPos, m_targetPos, timer);
            m_waterObject.transform.localScale = Vector3.Lerp(m_originalScale, m_targetScale, timer);
        }
        m_realKey.transform.position = m_fakeKey.transform.position;
        Destroy(m_fakeKey.gameObject);
        m_realKey.gameObject.SetActive(true);
    }

    void OnParticleCollision(GameObject other)
    {
        if (!m_filled)
        {
            StartCoroutine(FillWater());
            m_filled = true;

            Collider col = this.GetComponent<Collider>();
            if (col)
                col.enabled = false;
        }
    }
}
