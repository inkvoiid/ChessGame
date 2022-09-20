using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public float bgMusicVolume;
    public int whiteTeamMaxWidth;
    public int blackTeamMaxWidth;
    public int[] whitePieceType;
    public int[] whitePieceMaterial;
    public int[] whitePieceStartingX;
    public int[] whitePieceStartingY;

    public int[] blackPieceType;
    public int[] blackPieceMaterial;
    public int[] blackPieceStartingX;
    public int[] blackPieceStartingY;

    public GameData()
    {
        bgMusicVolume = 0.15f;
        whiteTeamMaxWidth = 8;
        blackTeamMaxWidth = 8;

        whitePieceType = new int[]
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

        whitePieceMaterial = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        whitePieceStartingX = new int[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7
        };

        whitePieceStartingY = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1
        };



        blackPieceType = new int[16]
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

        blackPieceMaterial = new int[16]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        blackPieceStartingX = new int[16]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7
        };

        blackPieceStartingY = new int[16]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1
        };
    }
}
