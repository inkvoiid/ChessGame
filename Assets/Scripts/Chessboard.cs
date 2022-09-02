using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpecialMove
{
    None = 0,
    EnPassant,
    Castling,
    Promotion
}

public class Chessboard : MonoBehaviour
{
    [Header("Art related")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private float deathSize = 0.5f;
    [SerializeField] private float deathSpacing = 0.3f;
    [SerializeField] private float dragOffset = 1.0f;
    [SerializeField] private GameObject victoryScreen;

    [Header("Prefabs and Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;

    [SerializeField] private Material[] basicWhiteMaterials;
    [SerializeField] private Material[] basicBlackMaterials;

    [SerializeField] private Material[] glassWhiteMaterials;
    [SerializeField] private Material[] glassBlackMaterials;

    // Logic
    private ChessPiece[,] chessPieces;
    public static ChessPiece currentlyDragging;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private List<ChessPiece> deadWhitePieces = new List<ChessPiece>();
    private List<ChessPiece> deadBlackPieces = new List<ChessPiece>();
    [SerializeField] private int tileCountX = 8;
    [SerializeField] private int tileCountY = 8;
    private GameObject[,] tiles;
    private static Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private bool isWhiteTurn;
    private SpecialMove specialMove;
    private List<Vector2Int[]> moveList = new List<Vector2Int[]>();

    private void Awake()
    {
        ResetTurn();

        GenerateAllTiles(tileSize, tileCountX, tileCountY);

        SpawnAllPieces();
        PositionAllPieces();
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Chessboard Tile", "Chessboard Tile Hover", "Chessboard Tile Highlight")))
        {
            // Gets the indices of the chessboard tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            // If we're hovering a tile after not hovering any tiles
            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Chessboard Tile Hover");
            }

            // If we were already hovering a tile, change the previous one
            if (currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Chessboard Tile Highlight") : LayerMask.NameToLayer("Chessboard Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Chessboard Tile Hover");
            }

            // If we press down on the mouse
            if (Input.GetMouseButtonDown(0))
            {
                if (chessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    // Is it our turn?
                    if ((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn) || (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn))
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];

                        // Get a list of where I can go, highlight tiles
                        availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, tileCountX, tileCountY);
                        // Get a list of special moves
                        specialMove = currentlyDragging.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves, new Vector2Int(tileCountX,tileCountY));

                        PreventCheck();
                        HighlightTiles();
                    }
                }
            }

            // If we release the mouse
            if (currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);

                if (!validMove)
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));

                currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Chessboard Tile Highlight") : LayerMask.NameToLayer("Chessboard Tile");
                currentHover = -Vector2Int.one;
            }

            if (currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }

        // If we're dragging a piece
        if (currentlyDragging)
        {
            Plane horizonPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if (horizonPlane.Raycast(ray, out distance))
            {
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
            }
        }
    }

    // Generate the board
    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];

        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(x + ", " + y);
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] triangles = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Chessboard Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    // Spawning of the pieces
    private void SpawnAllPieces()
    {
        chessPieces = new ChessPiece[tileCountX, tileCountY];

        int basicWhite = 0, basicBlack = 1;

        int teamWidth = 8;

        int startingX = (tileCountX - teamWidth) / 2;

        // WhiteTeam
        chessPieces[startingX + 0, 0] = SpawnSinglePiece(ChessPieceType.Rook, basicWhite);
        chessPieces[startingX + 1, 0] = SpawnSinglePiece(ChessPieceType.Knight, basicWhite);
        chessPieces[startingX + 2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, basicWhite);
        chessPieces[startingX + 3, 0] = SpawnSinglePiece(ChessPieceType.Queen, basicWhite);
        chessPieces[startingX + 4, 0] = SpawnSinglePiece(ChessPieceType.King, basicWhite);
        chessPieces[startingX + 5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, basicWhite);
        chessPieces[startingX + 6, 0] = SpawnSinglePiece(ChessPieceType.Knight, basicWhite);
        chessPieces[startingX + 7, 0] = SpawnSinglePiece(ChessPieceType.Rook, basicWhite);
        for (int i = startingX + 0; i < startingX + 8; i++)
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, basicWhite);

        // BlackTeam
        chessPieces[startingX + 0, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Rook, basicBlack);
        chessPieces[startingX + 1, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Knight, basicBlack);
        chessPieces[startingX + 2, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Bishop, basicBlack);
        chessPieces[startingX + 3, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Queen, basicBlack);
        chessPieces[startingX + 4, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.King, basicBlack);
        chessPieces[startingX + 5, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Bishop, basicBlack);
        chessPieces[startingX + 6, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Knight, basicBlack);
        chessPieces[startingX + 7, tileCountY - 1] = SpawnSinglePiece(ChessPieceType.Rook, basicBlack);
        for (int i = startingX + 0; i < startingX + 8; i++)
            chessPieces[i, tileCountY - 2] = SpawnSinglePiece(ChessPieceType.Pawn, basicBlack);


    }
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece piece = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

        piece.type = type;
        piece.team = team;

        Material[] teamMaterial;

        if (piece.team == 0)
        {
            teamMaterial = basicWhiteMaterials;
        }
        else
        {
            teamMaterial = basicBlackMaterials;
        }

        if (piece.type == ChessPieceType.Pawn)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[0];
        }
        else if (piece.type == ChessPieceType.Rook)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[1];
        }
        else if (piece.type == ChessPieceType.Knight)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[2];
        }
        else if (piece.type == ChessPieceType.Bishop)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[3];
        }
        else if (piece.type == ChessPieceType.Queen)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[4];
        }
        else if (piece.type == ChessPieceType.King)
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterial[5];
        }
        else
        {
            piece.GetComponent<MeshRenderer>().material = teamMaterials[team];
        }

        return piece;
    }

    // Positioning
    private void PositionAllPieces()
    {
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (chessPieces[x, y] != null)
                {
                    PositionSinglePiece(x, y, true);

                    chessPieces[x, y].startingPos = new Vector2Int(x, y);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y;
        chessPieces[x, y].SetPosition(GetTileCenter(x, y), force);
    }
    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }

    // Highlight Tiles
    private void HighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Chessboard Tile Highlight");
    }
    private void RemoveHighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Chessboard Tile");
        availableMoves.Clear();
    }

    // Checkmate
    private void Checkmate(int team)
    {
        DisplayVictory(team);
    }

    private void DisplayVictory(int winningTeam)
    {
        victoryScreen.SetActive(true);
        victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
    }

    public void OnResetButton()
    {
        // UI
        victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(1).gameObject.SetActive(false);
        victoryScreen.SetActive(false);

        // Field reset
        currentlyDragging = null;
        availableMoves.Clear();
        moveList.Clear();

        // Clean up
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (chessPieces[x, y] != null)
                    Destroy(chessPieces[x, y].gameObject);

                chessPieces[x, y] = null;
            }
        }

        for (int i = 0; i < deadWhitePieces.Count; i++)
            Destroy(deadWhitePieces[i].gameObject);

        for (int i = 0; i < deadBlackPieces.Count; i++)
            Destroy(deadBlackPieces[i].gameObject);

        deadWhitePieces.Clear();
        deadBlackPieces.Clear();

        SpawnAllPieces();
        PositionAllPieces();
        ResetTurn();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    // Special Moves
    private void ProcessSpecialMove()
    {
        if(specialMove == SpecialMove.EnPassant)
        {
            var newMove = moveList[moveList.Count - 1];
            ChessPiece myPawn = chessPieces[newMove[1].x, newMove[1].y];
            var targetPawnPosition = moveList[moveList.Count - 2];
            ChessPiece enemyPawn = chessPieces[targetPawnPosition[1].x, targetPawnPosition[1].y];

            if (myPawn.currentX == enemyPawn.currentX)
            {
                if(myPawn.currentY == enemyPawn.currentY - 1 || myPawn.currentY == enemyPawn.currentY + 1)
                {
                    if(enemyPawn.team == 0)
                    {
                        deadWhitePieces.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                            new Vector3(8 * tileSize, yOffset, -1 * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0, tileSize / 2)
                            + (Vector3.forward * deathSpacing) * deadWhitePieces.Count);
                    }
                    else
                    {
                        deadBlackPieces.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                            new Vector3(-1 * tileSize, yOffset, 8 * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0, tileSize / 2)
                            + (Vector3.back * deathSpacing) * deadBlackPieces.Count);
                    }
                    chessPieces[enemyPawn.currentX, enemyPawn.currentY] = null;
                }
            }
        }

        if(specialMove == SpecialMove.Promotion)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];

            if(targetPawn.type == ChessPieceType.Pawn)
            {
                if(targetPawn.team == 0 && lastMove[1].y == tileCountY - 1)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, 0);
                    newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
                    Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositionSinglePiece(lastMove[1].x, lastMove[1].y);
                }
                if (targetPawn.team == 1 && lastMove[1].y == 0)
                {
                    ChessPiece newQueen = SpawnSinglePiece(ChessPieceType.Queen, 1);
                    newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
                    Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositionSinglePiece(lastMove[1].x, lastMove[1].y);
                }
            }
        }

        if(specialMove == SpecialMove.Castling)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];

            int lastMoveDirectionX = lastMove[0].x - lastMove[1].x;
            int lastMoveDirectionY = lastMove[0].y - lastMove[1].y;

            //Left rook
            if(lastMoveDirectionX > 0)
            {
                for (int x = lastMove[1].x - 1; x >= 0; x--)
                {
                    if (chessPieces[x, lastMove[1].y] != null)
                    {
                        if (chessPieces[x, lastMove[1].y].type == ChessPieceType.Rook && chessPieces[x, lastMove[1].y].team == chessPieces[lastMove[1].x, lastMove[1].y].team)
                        {
                            ChessPiece rook = chessPieces[x, lastMove[1].y]; // Get the piece to the left of the King that just moved and store it
                            chessPieces[lastMove[1].x + 1, lastMove[1].y] = rook; // Set the rook to the tile on the right of the King
                            PositionSinglePiece(lastMove[1].x + 1, lastMove[1].y); // Actually move the physical rook
                            chessPieces[x, lastMove[1].y] = null; // Clear the tile that the rook moved from
                        }
                    }
                }
            }
            //Right rook
            else if(lastMoveDirectionX < 0)
            {
                for (int x = lastMove[1].x + 1; x < tileCountX; x++)
                {
                    if (chessPieces[x, lastMove[1].y] != null)
                    {
                        if (chessPieces[x, lastMove[1].y].type == ChessPieceType.Rook && chessPieces[x, lastMove[1].y].team == chessPieces[lastMove[1].x, lastMove[1].y].team)
                        {
                            ChessPiece rook = chessPieces[x, lastMove[1].y]; // Get the piece to the right of the King that just moved and store it
                            chessPieces[lastMove[1].x - 1, lastMove[1].y] = rook; // Set the rook to the tile on the left of the King
                            PositionSinglePiece(lastMove[1].x - 1, lastMove[1].y); // Actually move the physical rook
                            chessPieces[x, lastMove[1].y] = null; // Clear the tile that the rook moved from
                        }
                    }
                }
            }
            //Bottom rook
            else if(lastMoveDirectionY > 0)
            {
                for (int y = lastMove[1].y - 1; y >= 0; y--)
                {
                    if (chessPieces[lastMove[1].x, y] != null)
                    {
                        if (chessPieces[lastMove[1].x, y].type == ChessPieceType.Rook && chessPieces[lastMove[1].x, y].team == chessPieces[lastMove[1].x, lastMove[1].y].team)
                        {
                            ChessPiece rook = chessPieces[lastMove[1].x, y]; // Get the piece below the King that just moved and store it
                            chessPieces[lastMove[1].x, lastMove[1].y + 1] = rook; // Set the rook to the tile on the top of the King
                            PositionSinglePiece(lastMove[1].x, lastMove[1].y + 1); // Actually move the physical rook
                            chessPieces[lastMove[1].x, y] = null; // Clear the tile that the rook moved from
                        }
                    }
                }
            }
            //Top rook
            else if (lastMoveDirectionY < 0)
            {
                for (int y = lastMove[1].y + 1; y < tileCountY; y++)
                {
                    if (chessPieces[lastMove[1].x, y] != null)
                    {
                        if (chessPieces[lastMove[1].x, y].type == ChessPieceType.Rook && chessPieces[lastMove[1].x, y].team == chessPieces[lastMove[1].x, lastMove[1].y].team)
                        {
                            ChessPiece rook = chessPieces[lastMove[1].x, y]; // Get the piece above the King that just moved and store it
                            chessPieces[lastMove[1].x, lastMove[1].y - 1] = rook; // Set the rook to the tile on the bottom of the King
                            PositionSinglePiece(lastMove[1].x, lastMove[1].y - 1); // Actually move the physical rook
                            chessPieces[lastMove[1].x, y] = null; // Clear the tile that the rook moved from
                        }
                        break;
                    }
                }
            }
        }
    }

    private void PreventCheck()
    {
        ChessPiece targetKing = null;
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                if (chessPieces[x, y] != null)
                    if (chessPieces[x, y].type == ChessPieceType.King)
                        if (chessPieces[x, y].team == currentlyDragging.team)
                            targetKing = chessPieces[x, y];
            // Since availableMoves is passed in reference, we can delete moves from the list that put us in check
            SimulateMoveForSinglePiece(currentlyDragging, ref availableMoves, targetKing);
    }
    private void SimulateMoveForSinglePiece(ChessPiece piece, ref List<Vector2Int> moves, ChessPiece targetKing)
    {
        // Save the current values, to reset after the function call
        int actualX = piece.currentX;
        int actualY = piece.currentY;
        List<Vector2Int> movesToRemove = new List<Vector2Int>();

        // Go through all the moves, simulate tem and check if we're in check
        for (int i = 0; i < moves.Count; i++)
        {
            int simX = moves[i].x;
            int simY = moves[i].y;

            Vector2Int kingPositionThisSim = new Vector2Int(targetKing.currentX, targetKing.currentY);
            // Did we simulate the king's move
            if(piece.type == ChessPieceType.King)
                kingPositionThisSim = new Vector2Int(simX, simY);

            // Copy the [,] (2d array) and not a reference
            ChessPiece[,] simulation = new ChessPiece[tileCountX, tileCountY];
            List<ChessPiece> simAttackingPieces = new List<ChessPiece>();
            for (int x = 0; x < tileCountX; x++)
            {
                for (int y = 0; y < tileCountY; y++)
                {
                    if (chessPieces[x,y] != null) 
                    { 
                        simulation[x, y] = chessPieces[x, y];
                        if (simulation[x, y].team != piece.team)
                            simAttackingPieces.Add(simulation[x, y]);
                    }
                }
            }

            // Simulate that move
            simulation[actualX, actualY] = null;
            piece.currentX = simX;
            piece.currentY = simY;
            simulation[simX, simY] = piece;

            // Did the simulated move result in a piece getting taken
            var deadPiece = simAttackingPieces.Find(c => c.currentX == simX && c.currentY == simY);
            if (deadPiece != null)
                simAttackingPieces.Remove(deadPiece);

            // Get all the simulated attacking pieces moves

            List<Vector2Int> simMoves = new List<Vector2Int>();
            for (int a = 0; a < simAttackingPieces.Count; a++)
            {
                var pieceMoves = simAttackingPieces[a].GetAvailableMoves(ref simulation, tileCountX, tileCountY);
                for (int b = 0; b < pieceMoves.Count; b++)
                    simMoves.Add(pieceMoves[b]);
            }

            // Is the king in trouble? If so, remove the move
            if (ContainsValidMove(ref simMoves, kingPositionThisSim))
            {
                movesToRemove.Add(moves[i]);
            }

            // Restore actual piece data
            piece.currentX = actualX;
            piece.currentY = actualY;
        }
        // Remove from the current availableMove list
        for (int i = 0; i < movesToRemove.Count; i++)
            {
                moves.Remove(movesToRemove[i]);
            }
    }
    private bool CheckforCheckmate()
    {
        var lastMove = moveList[moveList.Count - 1];
        int targetTeam = (chessPieces[lastMove[1].x, lastMove[1].y].team == 0) ? 1 : 0;

        List<ChessPiece> attackingPieces = new List<ChessPiece>();
        List<ChessPiece> defendingPieces = new List<ChessPiece>();
        ChessPiece targetKing = null;
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                if (chessPieces[x, y] != null)
                {
                    if (chessPieces[x, y].team == targetTeam)
                    {
                        defendingPieces.Add(chessPieces[x, y]);

                        if (chessPieces[x, y].type == ChessPieceType.King)
                            targetKing = chessPieces[x, y];
                    }
                    else
                    {
                        attackingPieces.Add(chessPieces[x, y]);
                    }
                }

        // Is the king attacked right now?
        List<Vector2Int> currentAvailableMoves = new List<Vector2Int>();
        for (int i = 0; i < attackingPieces.Count; i++)
        {
            var pieceMoves = attackingPieces[i].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY);
            for (int b = 0; b < pieceMoves.Count; b++)
                currentAvailableMoves.Add(pieceMoves[b]);
        }

        // Are we in check right now?
        if (ContainsValidMove(ref currentAvailableMoves, new Vector2Int(targetKing.currentX, targetKing.currentY)))
        {
            // King is under attack, can we move something to help him?
            for (int i = 0; i < defendingPieces.Count; i++)
            {
                List<Vector2Int> defendingMoves = defendingPieces[i].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY);
                SimulateMoveForSinglePiece(defendingPieces[i], ref defendingMoves, targetKing);

                if(defendingMoves.Count != 0)
                {
                    return false;
                }
            }

            return true; // Checkmate exit
        }

        return false;
    }
    // Operations
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2Int pos)
    {
        for (int i = 0; i < moves.Count; i++)
            if (moves[i].x == pos.x && moves[i].y == pos.y)
                return true;

        return false;
    }

    private bool MoveTo(ChessPiece piece, int x, int y)
    {
        if (!ContainsValidMove(ref availableMoves, new Vector2Int(x, y))) 
            return false;

        Vector2Int previousPosition = new Vector2Int(piece.currentX, piece.currentY);

        // Is the target position occupied?
        if (chessPieces[x,y] != null)
        {
            ChessPiece otherPiece = chessPieces[x,y];

            if (piece.team == otherPiece.team)
            {
                return false;
            }

            // If it's the enemy team
            if (otherPiece.team == 0)
            {
                if (otherPiece.type == ChessPieceType.King)
                    Checkmate(1);

                deadWhitePieces.Add(otherPiece);
                otherPiece.SetScale(Vector3.one * deathSize);
                otherPiece.SetPosition(
                    new Vector3(8 * tileSize, yOffset, -1 * tileSize)
                    - bounds
                    + new Vector3(tileSize / 2, 0, tileSize / 2)
                    + (Vector3.forward * deathSpacing) * deadWhitePieces.Count);
            }
            else
            {
                if (otherPiece.type == ChessPieceType.King)
                    Checkmate(0);
                deadBlackPieces.Add(otherPiece);
                otherPiece.SetScale(Vector3.one * deathSize);
                otherPiece.SetPosition(
                    new Vector3(-1 * tileSize, yOffset, 8 * tileSize)
                    - bounds
                    + new Vector3(tileSize / 2, 0, tileSize / 2)
                    + (Vector3.back * deathSpacing) * deadBlackPieces.Count);
            }
        }

        chessPieces[x, y] = piece;
        chessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        NextTurn();
        moveList.Add(new Vector2Int[] { previousPosition, new Vector2Int(x,y) });

        ProcessSpecialMove();

        if(CheckforCheckmate())
        {
            Checkmate(piece.team);
        }

        return true;
    }

    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (tiles[x,y] == hitInfo)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one; // Returns -1, -1
    }

    // Get camera for other classes
    public static Camera getCurrentCamera()
    {
        return currentCamera;
    }

    // Change turn
    private void ResetTurn()
    {
        isWhiteTurn = true;
        GameObject.Find("TeamOverlay").GetComponent<Image>().color = Color.white;
    }
    private void NextTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        if (isWhiteTurn)
            GameObject.Find("TeamOverlay").GetComponent<Image>().color = Color.white;
        else
            GameObject.Find("TeamOverlay").GetComponent<Image>().color = new Color32(32, 32, 32, 255);
    }
}
