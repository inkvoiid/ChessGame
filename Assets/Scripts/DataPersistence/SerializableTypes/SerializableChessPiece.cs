using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableChessPiece : ISerializationCallbackReceiver
{
    [SerializeField] private int pieceType;
    [SerializeField] public string[] data;
    [SerializeField] public ChessPiece thisPiece;

    public void GetPiece(ChessPiece piece)
    {
        thisPiece = piece;
    }
    public void OnBeforeSerialize()
    {
        pieceType = (int) thisPiece.type;


    }
    
    public void OnAfterDeserialize()
    {
        thisPiece.type = (ChessPieceType) pieceType;
    }
}
