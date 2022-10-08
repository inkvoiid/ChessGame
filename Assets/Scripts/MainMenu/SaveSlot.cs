using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Save Slot")] 
    [SerializeField] private string saveSlotId = "";

    [Header("Content")] 
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI saveSlotName;
    [SerializeField] private TextMeshProUGUI saveSlotArmySize;
    [SerializeField] private TextMeshProUGUI saveSlotDateCreated;
    [SerializeField] private TextMeshProUGUI saveSlotLastPlayed;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponentInChildren<Button>();
    }

    public void SetData(GameData data)
    {
        // there's no data for the save slot id
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for the save slot id
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            saveSlotName.text = data.saveSlotName;
            saveSlotArmySize.text = data.whitePieceType.Count + " of " + data.whiteTeamMaxSquad + " Pieces";
            saveSlotDateCreated.text = data.dateCreated;
            saveSlotLastPlayed.text = data.lastPlayed;
        }
    }

    public string GetSaveSlotId()
    {
        return this.saveSlotId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
