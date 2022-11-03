using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CastleScreen : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject currentSquadDisplay;
    [SerializeField] private TMP_InputField slotName;
    [SerializeField] private TextMeshProUGUI slotNameButtonText;
    [SerializeField] private TextMeshProUGUI playerNameButtonText;
    [SerializeField] private LoadoutManager loadoutScreen;
    [SerializeField] private UpgradeManager upgradeScreen;

    private string saveSlotName;


    public static bool isWhiteTeam = true;

    public static List<int> whitePieceType;
    public static List<int> whitePieceMaterial;
    public static int whiteTeamMaxHeight;
    public static int whiteTeamMaxWidth;
    public static List<string> whitePieceAbilities;
    public static List<bool> whitePieceActive;
    public static List<int> whitePieceStartingX;
    public static List<int> whitePieceStartingY;
    public static int whiteTeamMaxSquad;

    public static List<int> blackPieceType;
    public static List<int> blackPieceMaterial;
    public static int blackTeamMaxHeight;
    public static int blackTeamMaxWidth;
    public static List<string> blackPieceAbilities;
    public static List<bool> blackPieceActive;
    public static List<int> blackPieceStartingX;
    public static List<int> blackPieceStartingY;
    public static int blackTeamMaxSquad;

    public void LoadData(GameData data)
    {
        this.saveSlotName = data.saveSlotName;

        whitePieceType = data.whitePieceType;
        whitePieceMaterial = data.whitePieceMaterial;
        whiteTeamMaxHeight = data.whiteTeamMaxHeight;
        whiteTeamMaxWidth = data.whiteTeamMaxWidth;
        whitePieceAbilities = data.whitePieceAbilities;
        whitePieceActive = data.whitePieceActive;
        whitePieceStartingX = data.whitePieceStartingX;
        whitePieceStartingY = data.whitePieceStartingY;
        whiteTeamMaxSquad = data.whiteTeamMaxSquad;

        blackPieceType = data.blackPieceType;
        blackPieceMaterial = data.blackPieceMaterial;
        blackTeamMaxHeight = data.blackTeamMaxHeight;
        blackTeamMaxWidth = data.blackTeamMaxWidth;
        blackPieceAbilities = data.blackPieceAbilities;
        blackPieceActive = data.blackPieceActive;
        blackPieceStartingX = data.blackPieceStartingX;
        blackPieceStartingY = data.blackPieceStartingY;
        blackTeamMaxSquad = data.blackTeamMaxSquad;

        slotName.text = saveSlotName;
        slotNameButtonText.text = saveSlotName;
        
        Debug.Log("Loaded from Castlescreen!");
    }

    public void SaveData(GameData data)
    {
        data.lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        data.saveSlotName = saveSlotName;


        data.whitePieceType = whitePieceType;
        data.whitePieceMaterial = whitePieceMaterial;
        data.whiteTeamMaxHeight = whiteTeamMaxHeight;
        data.whiteTeamMaxWidth = whiteTeamMaxWidth;
        data.blackPieceAbilities = whitePieceAbilities;
        data.whitePieceActive = whitePieceActive;
        data.whitePieceStartingX = whitePieceStartingX;
        data.whitePieceStartingY = whitePieceStartingY;
        data.whiteTeamMaxSquad = whiteTeamMaxWidth * whiteTeamMaxHeight;

        data.blackPieceType = blackPieceType;
        data.blackPieceMaterial = blackPieceMaterial;
        data.blackTeamMaxHeight = blackTeamMaxHeight;
        data.blackTeamMaxWidth = blackTeamMaxWidth;
        data.blackPieceAbilities = blackPieceAbilities;
        data.blackPieceActive = blackPieceActive;
        data.blackPieceStartingX = blackPieceStartingX;
        data.blackPieceStartingY = blackPieceStartingY;
        data.blackTeamMaxSquad = blackTeamMaxWidth * blackTeamMaxHeight;
        Debug.Log("Saved from CastleScreen Script!");
    }

    // Start is called before the first frame update
    void Start()
    {
        loadoutScreen.DeactivateMenu();
        upgradeScreen.DeactivateMenu();
        playerNameButtonText.text = "Player 1's Castle";
        UpdateCurrentSquadDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapCastles()
    {
        isWhiteTeam = !isWhiteTeam;
        int playerNum;
        if (isWhiteTeam)
            playerNum = 1;
        else
            playerNum = 2;
        playerNameButtonText.text = "Player " + playerNum + "'s Castle";
        UpdateCurrentSquadDisplay();
    }

    public void ChangeSlotName()
    {
        if (slotName.text == String.Empty)
            slotName.text = "An Unnnamed Slot";
        saveSlotName = slotName.text;
        slotNameButtonText.text = saveSlotName;
        DataPersistenceManager.instance.SaveGame();
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
        if (isWhiteTeam)
        {
            currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "Current Squad (" + whitePieceType.Count + " of " + whiteTeamMaxSquad + ")";
        }
        else
        {
            currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "Current Squad (" + blackPieceType.Count + " of " + blackTeamMaxSquad + ")";
        }
    }
}
