using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private GameObject toggleSoundButton;
    private GameObject backgroundMusic;
    private bool isMusicOn = true;

    [SerializeField] private Sprite imageOn;
    [SerializeField] private Sprite imageOff;

    private void Awake()
    {
        toggleSoundButton = GameObject.Find("ToggleSound");
        backgroundMusic = GameObject.Find("Background Music");
    }

    public void OnToggleSoundButton()
    {
        if (isMusicOn)
        {
            toggleSoundButton.transform.GetChild(0).GetComponentInChildren<Image>().sprite = imageOff;
        }
        else
        {
            toggleSoundButton.transform.GetChild(0).GetComponentInChildren<Image>().sprite = imageOn;
        }
        isMusicOn = !isMusicOn;
        backgroundMusic.GetComponent<AudioSource>().mute = !backgroundMusic.GetComponent<AudioSource>().mute;
    }
}
