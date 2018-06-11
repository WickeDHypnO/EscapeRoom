using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DigitalLockController : Photon.PunBehaviour, IPunObservable
{

    [SerializeField] private string m_password;
    [SerializeField] private string m_successText;

    [SerializeField] private UnityEvent m_eventsToTrigger;

    private string m_currentText;
    [SerializeField] private Text m_display;

    private bool m_invoked = false;
    private bool m_passed = false;

	void Start ()
    {
        ResetDisplay();
        StartCoroutine(DisplayRefresh());
    }
	
	private void UpdateDisplay()
    {
        m_display.text = m_currentText;
    }

    public void ResetDisplay()
    {
        m_currentText = string.Empty;
        UpdateDisplay();
    }

    public void Verify()
    {
        if (string.Compare(m_password, m_currentText) == 0)
        {
            Unlock();
            m_passed = true;
        }
        else
            ResetDisplay();
    }

    public void RecordChar(string newChar)
    {
        m_currentText += newChar;
        UpdateDisplay();
    }

    private void Unlock()
    {
        m_invoked = true;
        m_eventsToTrigger.Invoke();
        m_currentText = m_successText;
        UpdateDisplay();
    }

    IEnumerator DisplayRefresh()
    {
        while (!m_invoked)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateDisplay();
            if (m_passed && !m_invoked)
                Unlock();

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_currentText);
            stream.SendNext(m_passed);
        }
        else
        {
            m_currentText = (string)stream.ReceiveNext();
            m_passed = (bool)stream.ReceiveNext();
        }
    }

}
