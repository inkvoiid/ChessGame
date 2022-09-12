using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    static MusicController instance;
    private GameObject toggleSoundButton;

    [SerializeField] private Sprite imageOn;
    [SerializeField] private Sprite imageOff;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        RegisterSoundControl();
    }

    public void OnSoundSlider(float val)
    {
        instance.GetComponent<AudioSource>().volume = val;
    }

    public void RegisterSoundControl()
    {
        toggleSoundButton = GameObject.Find("ToggleSound");
        toggleSoundButton.GetComponentInChildren<Slider>().onValueChanged.AddListener((v) => OnSoundSlider(v));

        toggleSoundButton.GetComponentInChildren<Slider>().value = instance.GetComponent<AudioSource>().volume;
    }
}
