using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class CastleScreen : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject currentSquadDisplay;

    private List<int> whitePieceType;
    private List<int> whitePieceMaterial;

    private int whiteTeamMaxWidth;
    private int whiteTeamMaxHeight;
    private int whiteTeamMaxSquad;

    private int blackTeamMaxWidth;
    private int blackTeamMaxHeight;
    private int blackTeamMaxSquad;

    public void LoadData(GameData data)
    {

        this.whitePieceType = data.whitePieceType;
        this.whitePieceMaterial = data.whitePieceMaterial;

        this.whiteTeamMaxWidth = data.whiteTeamMaxWidth;
        this.whiteTeamMaxHeight = data.whiteTeamMaxHeight;
        this.whiteTeamMaxSquad = data.whiteTeamMaxWidth * data.blackTeamMaxHeight;

        this.blackTeamMaxWidth = data.blackTeamMaxWidth;
        this.blackTeamMaxHeight = data.blackTeamMaxHeight;
        this.blackTeamMaxSquad = data.blackTeamMaxWidth * data.blackTeamMaxHeight;
        Debug.Log("Loaded!");
        UpdateCurrentSquadDisplay();
    }

    public void SaveData(GameData data)
    {
        data.lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

        data.whitePieceType = this.whitePieceType;
        data.whitePieceMaterial = this.whitePieceMaterial;

        data.whiteTeamMaxWidth = this.whiteTeamMaxWidth;
        data.whiteTeamMaxHeight = this.whiteTeamMaxHeight;
        data.whiteTeamMaxSquad = this.whiteTeamMaxWidth * this.whiteTeamMaxHeight;

        data.blackTeamMaxWidth = this.blackTeamMaxWidth;
        data.blackTeamMaxHeight = this.blackTeamMaxHeight;
        data.blackTeamMaxSquad = this.blackTeamMaxWidth * this.blackTeamMaxWidth;
        Debug.Log("Saved from CastleScreen Script!");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void UpdateCurrentSquadDisplay()
    {
        currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "gay";
        currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "Current Squad (" + whitePieceType.Count + " of " + whiteTeamMaxSquad + ")";
    }
}
