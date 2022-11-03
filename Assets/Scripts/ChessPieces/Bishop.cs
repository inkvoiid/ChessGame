using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        // Don't modify any board code in here

        List<Vector2Int> r = new List<Vector2Int>();

        //Possibly add logic for walls in here so you can't move to them
        // (Check if wall before move or something)

        // Top Right
        for (int x = currentX + 1, y = currentY + 1; x < tileCountY && y < tileCountY; x++, y++)
        {
            if (board[x, y] == null)
            {
                r.Add(new Vector2Int(x, y));
            }
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        // Bottom Right
        for (int x = currentX + 1, y = currentY - 1; x < tileCountX && y >= 0; x++, y--)
        {
            if (board[x, y] == null)
            {
                r.Add(new Vector2Int(x, y));
            }
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        // Top Left
        for (int x = currentX - 1, y = currentY + 1; x >= 0 && y < tileCountY; x--, y++)
        {
            if (board[x, y] == null)
            {
                r.Add(new Vector2Int(x, y));
            }
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

                break;
            }
        }

        // Bottom Left
        for (int x = currentX - 1, y = currentY - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (board[x, y] == null)
            {
                r.Add(new Vector2Int(x, y));
            }
            else
            {
                if (board[x, y].team != team)
                    r.Add(new Vector2Int(x, y));

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
