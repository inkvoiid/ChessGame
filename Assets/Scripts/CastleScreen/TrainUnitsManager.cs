using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainUnitsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown material;
    [SerializeField] private TMP_Dropdown type;
    
    public void TrainPiece()
    {
        if (CastleScreen.isWhiteTeam)
        {
            if (CastleScreen.whitePieceType.Count < CastleScreen.whiteTeamMaxSquad)
            {
                CastleScreen.whitePieceType.Add(type.value + 1);
                CastleScreen.whitePieceMaterial.Add(material.value + 1);
                CastleScreen.whitePieceAbilities.Add("");
                CastleScreen.whitePieceActive.Add(true);
                CastleScreen.whitePieceStartingX.Add(-4);
                CastleScreen.whitePieceStartingY.Add(-4);
            }
        }
        else
        {
            if (CastleScreen.blackPieceType.Count < CastleScreen.blackTeamMaxSquad)
            {
                CastleScreen.blackPieceType.Add(type.value + 1);
                CastleScreen.blackPieceMaterial.Add(material.value + 1);
                CastleScreen.blackPieceAbilities.Add("");
                CastleScreen.blackPieceActive.Add(true);
                CastleScreen.blackPieceStartingX.Add(-4);
                CastleScreen.blackPieceStartingY.Add(-4);
            }
        }
        DataPersistenceManager.instance.SaveGame();
    }

    public void ActivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        this.gameObject.SetActive(false);
    }
}
