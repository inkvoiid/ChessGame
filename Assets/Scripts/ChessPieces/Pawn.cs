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

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves, Vector2Int boardDimensions)
    {
        int direction = (team == 0) ? 1 : -1;

        // En passant
        if (moveList.Count > 0)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            if (board[lastMove[1].x, lastMove[1].y].type == ChessPieceType.Pawn) // If the last piece was a pawn
            {
                if (Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2) // If the last move was a +2 in either direction
                {
                    if (board[lastMove[1].x, lastMove[1].y].team != team) // If the move was from the other team
                    {
                        if (lastMove[1].y == currentY) // If both pawns are on the same Y
                        {
                            if(lastMove[1].x == currentX - 1) // Landed left
                            {
                                availableMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }

                            if (lastMove[1].x == currentX + 1) // Landed right
                            {
                                availableMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                        }
                    }
                }
            }
        }

        // Promotion
        if((team == 0 && currentY == boardDimensions[1] -2) || team == 1 && currentY == 1)
        {

        }

        return SpecialMove.None;
    }
}
