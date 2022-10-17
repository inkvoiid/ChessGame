using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    MusicController bgMusic;
    [SerializeField] GameObject toggleSound;

    [Header("Menu Navigation")] 
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [SerializeField] private TextMeshProUGUI continueButtonSubtitle;

    private string saveSlotName;

    private void Start()
    {
        bgMusic = GameObject.Find("Background Music").GetComponent<MusicController>();
        bgMusic.RegisterSoundControl(toggleSound);
        if (bgMusic.GetComponent<AudioSource>().isPlaying == false)
            bgMusic.GetComponent<AudioSource>().Play();
    }
    public void OnClickNewGame()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnClickLoadGame()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
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
        this.saveSlotName = data.saveSlotName;
        InitializeVariablesAfterLoad();
    }

    private void InitializeVariablesAfterLoad()
    {
        continueButtonSubtitle.text = "Slot: " + saveSlotName;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
