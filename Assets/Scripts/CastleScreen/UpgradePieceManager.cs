using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePieceManager : MonoBehaviour
{
    private int index;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button[] abilityButtons;
    
    
    public void SetIndex(int ind)
    {
        // Store index
        index = ind;

        GenerateTitleText();
        
        UpdateUpgradeButton();
        EnableCorrectAbilities();
    }

    private void GenerateTitleText()
    {
        if (CastleScreen.isWhiteTeam)
        {
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
        else
        {
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
                4 => "Diamond",
                _ => "Basic"
            };

            title.text = material + " " + type + " (" + CastleScreen.blackPieceStartingX[index] + ", " + CastleScreen.blackPieceStartingY[index] + ")";
        }
    }

    public void AddAbility(Button button)
    {
        if (CastleScreen.isWhiteTeam)
        {
            CastleScreen.whitePieceAbilities[index] += button.GetComponentInChildren<TextMeshProUGUI>().text + " ";
        }
        else
        {
            CastleScreen.blackPieceAbilities[index] += button.GetComponentInChildren<TextMeshProUGUI>().text + " ";
        }
        EnableCorrectAbilities();
    }

    private void UpdateUpgradeButton()
    {
        if (CastleScreen.isWhiteTeam)
        {
            string upgradeText = "Upgrade to ";
            upgradeText += CastleScreen.whitePieceMaterial[index] switch
            {
                1 => "Ceramic",
                2 => "Stone",
                3 => "Diamond",
                _ => "Glass"
            };

            upgradeButton.interactable = true;
            if (CastleScreen.whitePieceMaterial[index] == 4)
            {
                upgradeText = "Fully Upgraded!";
                upgradeButton.interactable = false;
            }

            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeText;
        }
        else
        {
            string upgradeText = "Upgrade to ";
            upgradeText += CastleScreen.blackPieceMaterial[index] switch
            {
                1 => "Ceramic",
                2 => "Stone",
                3 => "Diamond",
                _ => "Glass"
            };

            upgradeButton.interactable = true;
            if (CastleScreen.blackPieceMaterial[index] == 4)
            {
                upgradeText = "Fully Upgraded!";
                upgradeButton.interactable = false;
            }

            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeText;
        }

    }

    public void EnableCorrectAbilities()
    {
        if (CastleScreen.isWhiteTeam)
        {
            foreach (var ability in abilityButtons)
            {
                ability.interactable = !CastleScreen.whitePieceAbilities[index]
                    .Contains(ability.GetComponentInChildren<TextMeshProUGUI>().text);
                ability.gameObject.SetActive(false);
            }

            if (CastleScreen.whitePieceType[index] == 1)
            {
                abilityButtons[0].gameObject.SetActive(true);
                abilityButtons[1].gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var ability in abilityButtons)
            {
                ability.interactable = !CastleScreen.blackPieceAbilities[index]
                    .Contains(ability.GetComponentInChildren<TextMeshProUGUI>().text);
                ability.gameObject.SetActive(false);
            }

            if (CastleScreen.blackPieceType[index] == 1)
            {
                abilityButtons[0].gameObject.SetActive(true);
                abilityButtons[1].gameObject.SetActive(true);
            }
        }
        
    }

    public void UpgradePiece()
    {
        if (CastleScreen.isWhiteTeam)
        {
            if (CastleScreen.whitePieceMaterial[index] < 4)
            {
                CastleScreen.whitePieceMaterial[index]++;
                UpdateUpgradeButton();
                GenerateTitleText();
            }
        }
        else
        {
            if (CastleScreen.blackPieceMaterial[index] < 4)
            {
                CastleScreen.blackPieceMaterial[index]++;
                UpdateUpgradeButton();
                GenerateTitleText();
            }
        }
    }
}
