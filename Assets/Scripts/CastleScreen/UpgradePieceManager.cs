using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePieceManager : MonoBehaviour
{
    private int index;
    [SerializeField] private TextMeshProUGUI title;
    public void SetIndex(int ind)
    {
        // Store index
        index = ind;

        // Generate Type of Piece
        string type = CastleScreen.whitePieceType[index] switch
        {
            1 => "Pawn",
            2 => "Rook",
            3 => "Knight",
            4 => "Bishop",
            5 => "Queen",
            6 => "King",
            _ => "Unknown"
        };

        string material = CastleScreen.whitePieceMaterial[index] switch
        {
            1 => "Glass",
            2 => "Ceramic",
            3 => "Stone",
            4 => "Diamond",
            _ => "Basic"
        };

        title.text = material + " " + type + " (" + CastleScreen.whitePieceStartingX[index] + ", " + CastleScreen.whitePieceStartingY[index] + ")";
    }
    public void AddAbility(Button button)
    {
        CastleScreen.whitePieceAbilities[index] += button.GetComponentInChildren<TextMeshProUGUI>().text + " ";
    }
}
