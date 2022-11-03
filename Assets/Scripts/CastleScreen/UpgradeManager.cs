using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private VerticalLayoutGroup pieceDisplay;
    [SerializeField] private GameObject loadoutTile;

    private void SetupUpgradeLoadout()
    {
        for (int i = 0; i < pieceDisplay.transform.childCount; i++)
        {
            GameObject.Destroy(pieceDisplay.transform.GetChild(i).gameObject);
        }

        if (CastleScreen.isWhiteTeam)
        {
            for (var index = 0; index < CastleScreen.whitePieceType.Count; index++)
            {
                Instantiate(loadoutTile, pieceDisplay.transform);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<UpgradePieceManager>().SetIndex(index);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<Image>().color = (index % 2 == 0) ? new Color32(15, 15, 15, 255) : new Color32(10, 10, 10, 255);
            }
        }
        else
        {
            for (var index = 0; index < CastleScreen.blackPieceType.Count; index++)
            {
                Instantiate(loadoutTile, pieceDisplay.transform);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<UpgradePieceManager>().SetIndex(index);
                pieceDisplay.transform.GetChild(pieceDisplay.transform.childCount - 1).gameObject.GetComponent<Image>().color = (index % 2 == 0) ? new Color32(15, 15, 15, 255) : new Color32(10, 10, 10, 255);
            }
        }
        
    }

    public void ActivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(true);
        SetupUpgradeLoadout();
    }

    public void DeactivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(false);
    }
}
