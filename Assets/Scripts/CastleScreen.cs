using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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
        data.whitePieceType = this.whitePieceType;
        data.whitePieceMaterial = this.whitePieceMaterial;

        data.whiteTeamMaxWidth = this.whiteTeamMaxWidth;
        data.whiteTeamMaxHeight = this.whiteTeamMaxHeight;
        data.whiteTeamMaxSquad = this.whiteTeamMaxWidth * this.whiteTeamMaxHeight;

        data.blackTeamMaxWidth = this.blackTeamMaxWidth;
        data.blackTeamMaxHeight = this.blackTeamMaxHeight;
        data.blackTeamMaxSquad = this.blackTeamMaxWidth * this.blackTeamMaxWidth;
        Debug.Log("Saved!");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentSquadDisplay()
    {
        Debug.Log(currentSquadDisplay.name);
        currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "gay";
        Debug.Log(whitePieceType.Count);
        Debug.Log(whiteTeamMaxSquad);
        currentSquadDisplay.GetComponent<TextMeshProUGUI>().text = "Current Squad (" + whitePieceType.Count + " of " + whiteTeamMaxSquad + ")";
    }
}
