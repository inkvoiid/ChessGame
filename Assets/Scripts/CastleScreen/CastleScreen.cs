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
    [SerializeField] private LoadoutManager loadoutScreen;
    [SerializeField] private UpgradeManager upgradeScreen;

    public static List<int> whitePieceType;
    public static List<int> whitePieceMaterial;
    public static int whiteTeamMaxHeight;
    public static int whiteTeamMaxWidth;
    public static List<string> whitePieceAbilities;
    public static List<bool> whitePieceActive;
    public static List<int> whitePieceStartingX;
    public static List<int> whitePieceStartingY;
    private static int whiteTeamMaxSquad;

    private int blackTeamMaxWidth;
    private int blackTeamMaxHeight;
    private int blackTeamMaxSquad;

    public void LoadData(GameData data)
    {

        whitePieceType = data.whitePieceType;
        whitePieceMaterial = data.whitePieceMaterial;

        whiteTeamMaxWidth = data.whiteTeamMaxWidth;
        whiteTeamMaxHeight = data.whiteTeamMaxHeight;
        whiteTeamMaxSquad = data.whiteTeamMaxWidth * data.blackTeamMaxHeight;

        this.blackTeamMaxWidth = data.blackTeamMaxWidth;
        this.blackTeamMaxHeight = data.blackTeamMaxHeight;
        this.blackTeamMaxSquad = data.blackTeamMaxWidth * data.blackTeamMaxHeight;
        Debug.Log("Loaded!");
        UpdateCurrentSquadDisplay();
    }

    public void SaveData(GameData data)
    {
        data.lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

        data.whitePieceType = whitePieceType;
        data.whitePieceMaterial = whitePieceMaterial;

        data.whiteTeamMaxWidth = whiteTeamMaxWidth;
        data.whiteTeamMaxHeight = whiteTeamMaxHeight;
        data.whiteTeamMaxSquad = whiteTeamMaxWidth * whiteTeamMaxHeight;

        data.blackTeamMaxWidth = this.blackTeamMaxWidth;
        data.blackTeamMaxHeight = this.blackTeamMaxHeight;
        data.blackTeamMaxSquad = this.blackTeamMaxWidth * this.blackTeamMaxWidth;
        Debug.Log("Saved from CastleScreen Script!");
    }

    // Start is called before the first frame update
    void Start()
    {
        loadoutScreen.DeactivateMenu();
        upgradeScreen.DeactivateMenu();
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
