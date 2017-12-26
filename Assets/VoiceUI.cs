using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceUI : MonoBehaviour
{
    public GameObject voiceIndicator;
    public GameObject otherVoiceIndicator;
    public PhotonVoiceRecorder photonVoiceRecorder;
    public PhotonVoiceSpeaker photonVoiceSpeaker;

    private void OnEnable()
    {
        StartCoroutine(FindOtherPlayerSpeaker());
    }

    IEnumerator FindOtherPlayerSpeaker()
    {
        yield return new WaitForSeconds(1f);
        foreach (PhotonVoiceSpeaker s in FindObjectsOfType<PhotonVoiceSpeaker>())
        {
            if (!s.photonView.isMine)
            {
                photonVoiceSpeaker = s;
            }
        }
    }

    void Update()
    {
        if (photonVoiceRecorder.IsTransmitting)
        {
            if (!voiceIndicator.activeInHierarchy)
            {
                voiceIndicator.SetActive(true);
            }
        }
        else
        {
            if (voiceIndicator.activeInHierarchy)
            {
                voiceIndicator.SetActive(false);
            }
        }
        if (photonVoiceSpeaker && photonVoiceSpeaker.IsPlaying)
        {
            if (!otherVoiceIndicator.activeInHierarchy)
            {
                otherVoiceIndicator.SetActive(true);
            }
        }
        else
        {
            if (otherVoiceIndicator.activeInHierarchy)
            {
                otherVoiceIndicator.SetActive(false);
            }
        }
    }
}
