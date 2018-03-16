using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour {

    public GameObject mainMenu;
    public Slider voiceVolume;
    public AudioMixer voiceMixer;

    private void OnEnable()
    {
        GetAllValues();
    }

    private void GetAllValues()
    {
        voiceVolume.value = PlayerPrefs.GetFloat("voiceVolume");
        ChangeVoiceVolume();
    }

    public void ChangeVoiceVolume()
    {
        PlayerPrefs.SetFloat("voiceVolume", voiceVolume.value);
        voiceMixer.SetFloat("voiceVolume", voiceVolume.value);
        if(voiceVolume.value <= -79)
        {
            voiceMixer.SetFloat("voiceVolume", -float.MaxValue);
        }
    }

	void Update () {
		if(Input.GetButtonDown("GameMenu"))
        {
            if(mainMenu.activeInHierarchy)
            {
                Camera.main.GetComponentInParent<RigidbodyFirstPersonController>().mouseEnabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                mainMenu.SetActive(false);
            }
            else
            {
                Camera.main.GetComponentInParent<RigidbodyFirstPersonController>().mouseEnabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                mainMenu.SetActive(true);
            }
        }
	}
}
