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

    // Added reference to custom ChessPieceType List so that I can find if any rooks have moved for castling
    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves, Vector2Int boardDimensions)
    {
        SpecialMove r = SpecialMove.None;
        int teamY = (team == 0) ? 0 : boardDimensions[1]-1;

        var kingMove = moveList.Find(m => m[0].x == startingPos.x && m[0].y == startingPos.y);

        // Up

        if (kingMove == null)
        {
            for (int i = currentY + 1; i < boardDimensions.y; i++)
            {
                if (board[currentX, i] != null)
                {
                    if (board[currentX, i].type == ChessPieceType.Rook && board[currentX, i].team == team)
                    {
                        if (board[currentX, i].startingPos.x == currentX && board[currentX, i].startingPos.y == i)
                        {
                            var topRook = moveList.Find(m => m[0].x == currentX && m[0].y == i);
                            if (topRook == null)
                            {
                                availableMoves.Add(new Vector2Int(currentX, currentY + 2));
                                r = SpecialMove.Castling;
                            }
                        }
                        break;
                    }

                    else
                    {
                        break;
                    }
                }
            }

            // Down
            for (int i = currentY - 1; i >= 0; i--)
            {
                if (board[currentX, i] != null)
                {
                    if (board[currentX, i].type == ChessPieceType.Rook && board[currentX, i].team == team)
                    {
                        if (board[currentX, i].startingPos.x == currentX && board[currentX, i].startingPos.y == i)
                        {
                            var bottomRook = moveList.Find(m => m[0].x == currentX && m[0].y == i);
                            if (bottomRook == null)
                            {
                                availableMoves.Add(new Vector2Int(currentX, currentY - 2));
                                r = SpecialMove.Castling;
                            }

                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Left
            for (int i = currentX - 1; i >= 0; i--)
            {
                if (board[i, currentY] != null)
                {
                    if (board[i, currentY].type == ChessPieceType.Rook && board[i, currentY].team == team)
                    {
                        if (board[i, currentY].startingPos.x == i && board[i, currentY].startingPos.y == currentY)
                        {
                            var leftRook = moveList.Find(m => m[0].x == i && m[0].y == currentY);
                            if (leftRook == null)
                            {
                                availableMoves.Add(new Vector2Int(currentX - 2, currentY));
                                r = SpecialMove.Castling;
                            }

                        }
                        break;
                    }

                    else
                    {
                        break;
                    }
                }
            }

            // Right
            for (int i = currentX + 1; i < boardDimensions.x; i++)
            {
                if (board[i, currentY] != null)
                {
                    if (board[i, currentY].type == ChessPieceType.Rook && board[i, currentY].team == team)
                    {
                        if (board[i, currentY].startingPos.x == i && board[i, currentY].startingPos.y == currentY)
                        {
                            var rightRook = moveList.Find(m => m[0].x == i && m[0].y == currentY);
                            if (rightRook == null)
                            {
                                availableMoves.Add(new Vector2Int(currentX + 2, currentY));
                                r = SpecialMove.Castling;
                            }
                        }
                        break;
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        return r;
    }
}
