using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour, IDataPersistence
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
    }

    public void OnSoundSlider(float val)
    {
        instance.GetComponent<AudioSource>().volume = val;
    }

    public void RegisterSoundControl(GameObject ts)
    {
        toggleSoundButton = ts;
        toggleSoundButton.GetComponentInChildren<Slider>().onValueChanged.AddListener((v) => OnSoundSlider(v));

        toggleSoundButton.GetComponentInChildren<Slider>().value = instance.GetComponent<AudioSource>().volume;
    }

    public void LoadData(GameData data)
    {
        instance.GetComponent<AudioSource>().volume = data.bgMusicVolume;
    }

    public void SaveData(GameData data)
    {
        data.bgMusicVolume = instance.GetComponent<AudioSource>().volume;
    }
}
