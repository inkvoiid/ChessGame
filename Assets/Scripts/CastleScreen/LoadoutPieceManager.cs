using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutPieceManager : MonoBehaviour
{
    private int index;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI abilities;
    [SerializeField] private TMP_InputField xInput;
    [SerializeField] private TMP_InputField yInput;
    public void SetIndex(int ind)
    {
        // Store index
        index = ind;

        // Fill X, Y input fields with correct values
        xInput.text = ""+ CastleScreen.whitePieceStartingX[index];
        yInput.text = "" + CastleScreen.whitePieceStartingY[index];

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

        title.text = material + " " + type;

        abilities.text = CastleScreen.whitePieceAbilities[index];
    }

    public void ResetPieceLocation()
    {
        // Fill X, Y input fields with correct values
        xInput.text = "" + CastleScreen.whitePieceStartingX[index];
        yInput.text = "" + CastleScreen.whitePieceStartingY[index];
    }

    public void ChangePieceLocation()
    {
        bool spotAlreadyTaken = false;
        int desiredX, desiredY;
        bool validX = int.TryParse(xInput.text, out desiredX);
        bool validY = int.TryParse(yInput.text, out desiredY);
        if (!validX || !validY)
        {
            Debug.Log("Not a valid spot");
            return;
        }
        for (int i = 0; i < CastleScreen.whitePieceType.Count; i++)
        {
            if(i==index)
                continue;
            if (CastleScreen.whitePieceActive[i])
            {
                if (CastleScreen.whitePieceStartingX[i] == desiredX &&
                    CastleScreen.whitePieceStartingY[i] == desiredY)
                {
                    spotAlreadyTaken = true;
                    Debug.Log("Spot already taken");
                    break;
                }
            }
        }

        if (!spotAlreadyTaken)
        {
            Debug.Log("Spot all clear");
            CastleScreen.whitePieceStartingX[index] = desiredX;
            CastleScreen.whitePieceStartingY[index] = desiredY;
        }
    }
}