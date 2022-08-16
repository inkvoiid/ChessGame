using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        // Don't modify any board code in here

        List<Vector2Int> r = new List<Vector2Int>();

        //Possibly add logic for walls in here so you can't move to them
        // (Check if wall before move or something)

        // Top Right
        if(currentY + 1 < tileCountY)
        {
            if (board[currentX + 1, currentY + 1] == null)
                r.Add(new Vector2Int(currentX + 1, currentY + 1));
            else if(board[currentX + 1, currentY + 1].team != team)
                r.Add(new Vector2Int(currentX + 1, currentY + 1));
        }
        // Right
        if (currentX + 1 < tileCountX)
        {
            if (board[currentX + 1, currentY] == null)
                r.Add(new Vector2Int(currentX + 1, currentY));
            else if (board[currentX + 1, currentY].team != team)
                r.Add(new Vector2Int(currentX + 1, currentY));
        }
        // Bottom Right
        if (currentY - 1 >= 0)
        {
            if (board[currentX + 1, currentY - 1] == null)
                r.Add(new Vector2Int(currentX + 1, currentY - 1));
            else if (board[currentX + 1, currentY - 1].team != team)
                r.Add(new Vector2Int(currentX + 1, currentY - 1));
        }

        // Top Left
        if (currentY + 1 < tileCountY)
        {
            if (board[currentX - 1, currentY + 1] == null)
                r.Add(new Vector2Int(currentX - 1, currentY + 1));
            else if (board[currentX - 1, currentY + 1].team != team)
                r.Add(new Vector2Int(currentX - 1, currentY + 1));
        }
        // Left
        if (currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY] == null)
                r.Add(new Vector2Int(currentX - 1, currentY));
            else if (board[currentX - 1, currentY].team != team)
                r.Add(new Vector2Int(currentX - 1, currentY));
        }
        // Bottom Left
        if (currentY - 1 >= 0)
        {
            if (board[currentX - 1, currentY - 1] == null)
                r.Add(new Vector2Int(currentX - 1, currentY - 1));
            else if (board[currentX - 1, currentY - 1].team != team)
                r.Add(new Vector2Int(currentX - 1, currentY - 1));
        }

        // Up
        if (currentY + 1 < tileCountY)
        {
            if (board[currentX, currentY + 1] == null)
                r.Add(new Vector2Int(currentX, currentY + 1));
            else if (board[currentX, currentY + 1].team != team)
                r.Add(new Vector2Int(currentX, currentY + 1));
        }

        // Down
        if (currentY - 1 >= 0)
        {
            if (board[currentX, currentY - 1] == null)
                r.Add(new Vector2Int(currentX, currentY - 1));
            else if (board[currentX, currentY - 1].team != team)
                r.Add(new Vector2Int(currentX, currentY - 1));
        }

        return r;
    }
}
