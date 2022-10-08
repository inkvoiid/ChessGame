using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Menu Navigation")] 
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")] 
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnClickedSaveSlot(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        
        DataPersistenceManager.instance.ChangeSelectedSaveSlot(saveSlot.GetSaveSlotId());

        if(!isLoadingGame)
            DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync(1);
    }

    public void OnClickedBack()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        // Set the menu to be active
        this.gameObject.SetActive(true);

        // Set mode
        this.isLoadingGame = isLoadingGame;

        // Load all of the save slots that exist
        Dictionary<string, GameData> saveSlotsGameData = DataPersistenceManager.instance.GetAllSaveSlotsGameData();

        // Loop through each save slot in the UI and set the content appropriately
        foreach (var saveSlot in saveSlots)
        {
            GameData saveSlotData = null;
            saveSlotsGameData.TryGetValue(saveSlot.GetSaveSlotId(), out saveSlotData);
            saveSlot.SetData(saveSlotData);

            if (saveSlotData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void DisableMenuButtons()
    {
        foreach (var saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
