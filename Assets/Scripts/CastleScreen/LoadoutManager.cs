using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup pieceDisplay;
    [SerializeField] private GameObject loadoutTile;



    [SerializeField] private TextMeshProUGUI maxX;
    [SerializeField] private TextMeshProUGUI maxY;
    [SerializeField] private Button increaseMaxX;
    [SerializeField] private Button increaseMaxY;


    private void SetupLoadout()
    {
        for (int i = 0; i < pieceDisplay.transform.childCount; i++)
        {
            GameObject.Destroy(pieceDisplay.transform.GetChild(i).gameObject);
        }

        if (CastleScreen.isWhiteTeam)
        {
            maxX.text = "Max X: " + CastleScreen.whiteTeamMaxWidth;
            maxY.text = "Max Y: " + CastleScreen.whiteTeamMaxHeight;
            for (int index = 0; index < CastleScreen.whitePieceType.Count; index++)
            {
                Instantiate(loadoutTile, pieceDisplay.transform);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<LoadoutPieceManager>().SetIndex(index);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<Image>().color = (index % 2 == 0) ? new Color32(15, 15, 15, 255) : new Color32(10, 10, 10, 255);
            }
        }
        else
        {
            maxX.text = "Max X: " + CastleScreen.blackTeamMaxWidth;
            maxY.text = "Max Y: " + CastleScreen.blackTeamMaxHeight;
            for (var index = 0; index < CastleScreen.blackPieceType.Count; index++)
            {
                Instantiate(loadoutTile, pieceDisplay.transform);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<LoadoutPieceManager>().SetIndex(index);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<Image>().color = (index % 2 == 0) ? new Color32(15, 15, 15, 255) : new Color32(10, 10, 10, 255);
            }
        }
    }

    public void ActivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(true);
        SetupLoadout();
    }

    public void DeactivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(false);
    }
    
    public void IncreaseMaxX()
    {
        if (CastleScreen.isWhiteTeam)
        {
            if (CastleScreen.whiteTeamMaxWidth < 14)
            {
                CastleScreen.whiteTeamMaxWidth++;
                maxX.text = "Max X: " + CastleScreen.whiteTeamMaxWidth;
            }
        }
        else
        {
            if (CastleScreen.blackTeamMaxWidth < 14)
            {
                CastleScreen.blackTeamMaxWidth++;
                maxX.text = "Max X: " + CastleScreen.blackTeamMaxWidth;
            }
        }
    }

    public void IncreaseMaxY()
    {
        if (CastleScreen.isWhiteTeam)
        {
            if (CastleScreen.whiteTeamMaxHeight < 4)
            {
                CastleScreen.whiteTeamMaxHeight++;
                maxY.text = "Max Y: " + CastleScreen.whiteTeamMaxHeight;
            }
        }
        else
        {
            if (CastleScreen.blackTeamMaxHeight < 4)
            {
                CastleScreen.blackTeamMaxHeight++;
                maxY.text = "Max Y: " + CastleScreen.blackTeamMaxHeight;
            }
        }
    }
}
