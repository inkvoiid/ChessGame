using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    MusicController bgMusic;
    [SerializeField] GameObject toggleSound;
    private void Start()
    {
        bgMusic = GameObject.Find("Background Music").GetComponent<MusicController>();
        bgMusic.RegisterSoundControl(toggleSound);
        if (bgMusic.GetComponent<AudioSource>().isPlaying == false)
            bgMusic.GetComponent<AudioSource>().Play();
    }
    public void OnClickNewGame()
    {
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void LoadScene(int sceneNum)
    {
        try
        {
            SceneManager.LoadSceneAsync(sceneNum);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError("Tried to load a scene of index " + sceneNum + ", which isn't a valid index\n" + e);
        }
    }

    public void SaveData(GameData data)
    {

    }

    public void LoadData(GameData data)
    {
        InitializeVariablesAfterLoad();
    }

    private void InitializeVariablesAfterLoad()
    {
        
    }
}
