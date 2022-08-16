using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        // Don't modify any board code in here

        List<Vector2Int> r = new List<Vector2Int>();

        int direction = (team == 0) ? 1 : -1;

        //Possibly add logic for walls in here so you can't move to them
        // (Check if wall before move or something)

        // One in front
        if (board[currentX, currentY + direction] == null)
            r.Add(new Vector2Int(currentX, currentY + direction));

        // Two in front
        if (board[currentX, currentY + direction] == null)
        {
            // White Team
            if(team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null)
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));

            // Black Team
            if (team == 1 && currentY == (tileCountY-2) && board[currentX, currentY + (direction * 2)] == null)
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
        }

        // Kill move

        // Right
        if (currentX != tileCountX - 1)
            if (board[currentX+1, currentY+direction] != null && board[currentX + 1, currentY + direction].team != team)
                r.Add(new Vector2Int(currentX+1, currentY + direction));
        // Left
        if (currentX != 0)
            if (board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team)
                r.Add(new Vector2Int(currentX - 1, currentY + direction));


        return r;
    }
}
