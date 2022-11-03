using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup pieceDisplay;
    [SerializeField] private GameObject loadoutTile;


    private void SetupLoadout()
    {
        for (int i = 0; i < pieceDisplay.transform.childCount; i++)
        {
            GameObject.Destroy(pieceDisplay.transform.GetChild(i).gameObject);
        }

        if (CastleScreen.isWhiteTeam)
        {
            for (int index = 0; index < CastleScreen.whitePieceType.Count; index++)
            {
                Instantiate(loadoutTile, pieceDisplay.transform);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<LoadoutPieceManager>().SetIndex(index);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<Image>().color = (index % 2 == 0) ? new Color32(15, 15, 15, 255) : new Color32(10, 10, 10, 255);
            }
        }
        else
        {
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
}
