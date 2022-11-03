using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPieceManager : MonoBehaviour
{
    private int index;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI abilities;
    [SerializeField] private Toggle isActive;
    [SerializeField] private TMP_InputField xInput;
    [SerializeField] private TMP_InputField yInput;
    public void SetIndex(int ind)
    {
        // Store index
        index = ind;
        if (CastleScreen.isWhiteTeam)
        {
            // Fill X, Y input fields with correct values
            xInput.text = CastleScreen.whitePieceStartingX[index].ToString();
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
                4 => "Metal",
                5 => "Diamond",
                _ => "Basic"
            };
            
            if (CastleScreen.whitePieceType[index] == 6)
            {
                CastleScreen.whitePieceActive[index] = true;
                Destroy(isActive.gameObject);
            }

            title.text = material + " " + type;
            
            abilities.text = CastleScreen.whitePieceAbilities[index];
        }
        else
        {
            // Fill X, Y input fields with correct values
            xInput.text = "" + CastleScreen.blackPieceStartingX[index];
            yInput.text = "" + CastleScreen.blackPieceStartingY[index];

            // Generate Type of Piece
            string type = CastleScreen.blackPieceType[index] switch
            {
                1 => "Pawn",
                2 => "Rook",
                3 => "Knight",
                4 => "Bishop",
                5 => "Queen",
                6 => "King",
                _ => "Unknown"
            };

            string material = CastleScreen.blackPieceMaterial[index] switch
            {
                1 => "Glass",
                2 => "Ceramic",
                3 => "Stone",
                4 => "Metal",
                5 => "Diamond",
                _ => "Basic"
            };

            if (CastleScreen.blackPieceType[index] == 6)
            {
                CastleScreen.blackPieceActive[index] = true;
                Destroy(isActive.gameObject);
            }

            title.text = material + " " + type;

            abilities.text = CastleScreen.blackPieceAbilities[index];
        }

        SetInputFieldActivity();
    }

    public void ResetPieceLocation()
    {
        if (CastleScreen.isWhiteTeam)
        {
            // Fill X, Y input fields with correct values
            xInput.text = "" + CastleScreen.whitePieceStartingX[index];
            yInput.text = "" + CastleScreen.whitePieceStartingY[index];
        }
        else
        {
            // Fill X, Y input fields with correct values
            xInput.text = "" + CastleScreen.blackPieceStartingX[index];
            yInput.text = "" + CastleScreen.blackPieceStartingY[index];
        }
    }

    public void SetInputFieldActivity()
    {
        if (CastleScreen.isWhiteTeam)
        {
            isActive.isOn = CastleScreen.whitePieceActive[index];
        }
        else
        {
            isActive.isOn = CastleScreen.blackPieceActive[index];
        }

        if (isActive.isOn)
        {
            xInput.interactable = true;
            yInput.interactable = true;
        }
        else
        {
            xInput.interactable = false;
            yInput.interactable = false;
        }
    }

    public void ChangeInputFieldActivity()
    {
        if (CastleScreen.isWhiteTeam)
        {
            CastleScreen.whitePieceActive[index] = isActive.isOn;
        }
        else
        {
            CastleScreen.blackPieceActive[index] = isActive.isOn;
        }

        if (isActive.isOn)
        {
            xInput.interactable = true;
            yInput.interactable = true;
        }
        else
        {
            xInput.interactable = false;
            yInput.interactable = false;
        }
    }

    public void ChangePieceLocation()
    {
        int desiredX, desiredY;
        bool validX = int.TryParse(xInput.text, out desiredX);
        bool validY = int.TryParse(yInput.text, out desiredY);
        if (!validX || !validY)
        {
            Debug.Log("Not a valid spot");
            return;
        }

        if (CastleScreen.isWhiteTeam)
        {
            for (int i = 0; i < CastleScreen.whitePieceType.Count; i++)
            {
                if (i == index)
                    continue;
                if (CastleScreen.whitePieceActive[i])
                {
                    if (CastleScreen.whitePieceStartingX[i] == desiredX &&
                        CastleScreen.whitePieceStartingY[i] == desiredY)
                    {
                        int tempX = CastleScreen.whitePieceStartingX[index];
                        int tempY = CastleScreen.whitePieceStartingY[index];
                        Debug.Log("Swapped two piece locations");
                        break;
                    }
                }
            }

            Debug.Log("Spot all clear");
            CastleScreen.whitePieceStartingX[index] = desiredX;
            CastleScreen.whitePieceStartingY[index] = desiredY;
        }
        else
        {
            for (int i = 0; i < CastleScreen.blackPieceType.Count; i++)
            {
                if (i == index)
                    continue;
                if (CastleScreen.blackPieceActive[i])
                {
                    if (CastleScreen.blackPieceStartingX[i] == desiredX &&
                        CastleScreen.blackPieceStartingY[i] == desiredY)
                    {
                        int tempX = CastleScreen.blackPieceStartingX[index];
                        int tempY = CastleScreen.blackPieceStartingY[index];
                        Debug.Log("Swapped two piece locations");
                        break;
                    }
                }
            }

            Debug.Log("Spot all clear");
            CastleScreen.blackPieceStartingX[index] = desiredX;
            CastleScreen.blackPieceStartingY[index] = desiredY;
        }
    }
}
