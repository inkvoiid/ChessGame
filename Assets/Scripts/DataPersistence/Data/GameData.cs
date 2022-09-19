using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public float bgMusicVolume;
    public int whiteTeamMaxWidth;
    public int blackTeamMaxWidth;
    public SerializableDictionary<Vector2Int, ChessPiece> whiteTeamChessPieces;
    public SerializableDictionary<Vector2Int, ChessPiece> blackTeamChessPieces;

    public GameData()
    {
        bgMusicVolume = 0.15f;
        whiteTeamMaxWidth = 8;
        blackTeamMaxWidth = 8;
        whiteTeamChessPieces = new SerializableDictionary<Vector2Int, ChessPiece>();
        blackTeamChessPieces = new SerializableDictionary<Vector2Int, ChessPiece>();
        //whiteTeamChessPieces.Add(new Vector2Int(0,0),new King());
    }
}
