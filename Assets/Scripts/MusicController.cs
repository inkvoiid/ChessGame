using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour, IDataPersistence
{
    static MusicController instance;
    private GameObject toggleSoundButton;
    private int currentSong, previousSong = - 1;
    private System.Random rand = new System.Random();
    [SerializeField] private AudioClip[] songs = new AudioClip[10];

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
        currentSong = rand.Next(0, songs.Length);
        this.GetComponent<AudioSource>().clip = songs[currentSong];
        previousSong = currentSong;
    }

    private void Update()
    {
        if (!transform.GetComponent<AudioSource>().isPlaying && transform.GetComponent<AudioSource>().time == 0.00)
        {
            do
            {
                currentSong = rand.Next(0, songs.Length);
            } 
            while (currentSong == previousSong);

            this.GetComponent<AudioSource>().clip = songs[currentSong];
            this.GetComponent<AudioSource>().Play();
            previousSong = currentSong;

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
