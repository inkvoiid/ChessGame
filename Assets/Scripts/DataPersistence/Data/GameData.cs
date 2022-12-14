using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public string saveSlotName;
    public string dateCreated;
    public string lastPlayed;
    public float bgMusicVolume;

    public int whiteTeamGold;

    public int whiteTeamMaxWidth;
    public int whiteTeamMaxHeight;
    public int whiteTeamMaxSquad;

    public int blackTeamGold;

    public int blackTeamMaxWidth;
    public int blackTeamMaxHeight;
    public int blackTeamMaxSquad;

    public List<int> whitePieceType;
    public List<int> whitePieceMaterial;
    public List<int> whitePieceStartingX;
    public List<int> whitePieceStartingY;
    public List<string> whitePieceAbilities;
    public List<bool> whitePieceActive;

    public List<int> blackPieceType;
    public List<int> blackPieceMaterial;
    public List<int> blackPieceStartingX;
    public List<int> blackPieceStartingY;
    public List<string> blackPieceAbilities;
    public List<bool> blackPieceActive;

    public GameData()
    {
        saveSlotName = "An Unnamed Slot";
        dateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        bgMusicVolume = 0.15f;

        whiteTeamGold = 0;

        whiteTeamMaxWidth = 8;
        whiteTeamMaxHeight = 2;
        whiteTeamMaxSquad = whiteTeamMaxWidth * whiteTeamMaxHeight;

        blackTeamGold = 0;

        blackTeamMaxWidth = 8;
        blackTeamMaxHeight = 2;
        blackTeamMaxSquad = blackTeamMaxWidth * blackTeamMaxHeight;

        whitePieceType = new List<int>
        {
            (int)ChessPieceType.Rook, 
            (int)ChessPieceType.Knight,
            (int)ChessPieceType.Bishop,
            (int)ChessPieceType.Queen,
            (int)ChessPieceType.King,
            (int)ChessPieceType.Bishop,
            (int)ChessPieceType.Knight,
            (int)ChessPieceType.Rook,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn
        };

        whitePieceMaterial = new List<int>
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        };

        whitePieceStartingX = new List<int>
        {
            0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7
        };

        whitePieceStartingY = new List<int>
        {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1
        };

        whitePieceAbilities = new List<string>
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };

        whitePieceActive = new List<bool>
        {
            true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true
        };

        blackPieceType = new List<int>
        {
            (int)ChessPieceType.Rook,
            (int)ChessPieceType.Knight,
            (int)ChessPieceType.Bishop,
            (int)ChessPieceType.King,
            (int)ChessPieceType.Queen,
            (int)ChessPieceType.Bishop,
            (int)ChessPieceType.Knight,
            (int)ChessPieceType.Rook,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn,
            (int)ChessPieceType.Pawn
        };

        blackPieceMaterial = new List<int>
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        };

        blackPieceStartingX = new List<int>
        {
            0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7
        };

        blackPieceStartingY = new List<int>
        {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1
        };

        blackPieceAbilities = new List<string>
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };

        blackPieceActive = new List<bool>
        {
            true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true
        };
    }
}
