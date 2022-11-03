using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        // Don't modify any board code in here

        List<Vector2Int> r = new List<Vector2Int>();

        //Possibly add logic for walls in here so you can't move to them
        // (Check if wall before move or something)

        // Up
        for (int i = currentY + 1; i < tileCountY; i++)
        {
            if (board[currentX, i] == null)
            {
                r.Add(new Vector2Int(currentX, i));
            }
            if (board[currentX, i] != null)
            {
                if (board[currentX, i].team != team)
                    r.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        // Down
        for (int i = currentY - 1; i >= 0; i--)
        {
            if (board[currentX, i] == null)
            {
                r.Add(new Vector2Int(currentX, i));
            }
            if (board[currentX, i] != null)
            {
                if(board[currentX, i].team != team)
                    r.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        // Left
        for (int i = currentX - 1; i >= 0; i--)
        {
            if (board[i, currentY] == null)
            {
                r.Add(new Vector2Int(i, currentY));
            }
            if (board[i, currentY] != null)
            {
                if (board[i, currentY].team != team)
                    r.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        // Right
        for (int i = currentX + 1; i < tileCountX; i++)
        {
            if (board[i, currentY] == null)
            {
                r.Add(new Vector2Int(i, currentY));
            }
            if (board[i, currentY] != null)
            {
                if (board[i, currentY].team != team)
                    r.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        // Abilities

        //King on a Horse
        if (abilityKnighted == true)
        {
            // Top right
            int x = currentX + 1;
            int y = currentY + 2;
            if (x < tileCountX && y < tileCountY)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = currentX + 2;
            y = currentY + 1;
            if (x < tileCountX && y < tileCountY)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            // Top Left
            x = currentX - 1;
            y = currentY + 2;
            if (x >= 0 && y < tileCountY)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = currentX - 2;
            y = currentY + 1;
            if (x >= 0 && y < tileCountY)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            // Bottom right
            x = currentX + 1;
            y = currentY - 2;
            if (x < tileCountX && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = currentX + 2;
            y = currentY - 1;
            if (x < tileCountX && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            // Bottom Left
            x = currentX - 1;
            y = currentY - 2;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

            x = currentX - 2;
            y = currentY - 1;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));
        }

        return r;
    }
}
