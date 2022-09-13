using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    MusicController bgMusic;
    [SerializeField] GameObject toggleSound;
    private void Awake()
    {
        bgMusic = GameObject.Find("Background Music").GetComponent<MusicController>(); 
        if (bgMusic.GetComponent<AudioSource>().isPlaying == false)
            bgMusic.GetComponent<AudioSource>().Play();
        bgMusic.RegisterSoundControl(toggleSound);
    }
    public void OnClickPlay()
    {

    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickScene2()
    {
        SceneManager.LoadScene(2);
    }
}
