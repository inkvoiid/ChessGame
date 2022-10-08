using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SpecialMove
{
    None = 0,
    EnPassant,
    Castling,
    Promotion
}

public class Chessboard : MonoBehaviour, IDataPersistence
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
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject checkNotif;

    [Header("Prefabs and Materials")]
    [SerializeField] private GameObject[] prefabs;

    // Chess Piece Material Types
    [SerializeField] private Material[] basicWhiteMaterials;
    [SerializeField] private Material[] basicBlackMaterials;

    [SerializeField] private Material[] glassWhiteMaterials;
    [SerializeField] private Material[] glassBlackMaterials;

    [SerializeField] private Material[] ceramicWhiteMaterials;
    [SerializeField] private Material[] ceramicBlackMaterials;

    [SerializeField] private Material[] stoneWhiteMaterials;
    [SerializeField] private Material[] stoneBlackMaterials;

    [SerializeField] private Material[] diamondWhiteMaterials;
    [SerializeField] private Material[] diamondBlackMaterials;

    // Logic
    private ChessPiece[,] chessPieces;
    public static ChessPiece currentlyDragging;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private List<ChessPiece> deadWhitePieces = new List<ChessPiece>();
    private List<ChessPiece> deadBlackPieces = new List<ChessPiece>();
    private Dictionary<ChessPiece, Vector2Int> piecesThatPutKingsInCheck = new Dictionary<ChessPiece, Vector2Int>();
    [SerializeField] private int tileCountX = 8;
    [SerializeField] private int tileCountY = 8;
    private GameObject[,] tiles;
    private static Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private bool isWhiteTurn;
    private SpecialMove specialMove;
    private List<Vector2Int[]> moveList = new List<Vector2Int[]>();

    // Music Related
    MusicController bgMusic;
    [SerializeField] private GameObject toggleSound;

    // Custom Chess Team
    private int blackTeamMaxWidth;
    private int whiteTeamMaxWidth;

    private List<int> whitePieceActualIndex;
    private List<int> blackPieceActualIndex;

    private List<int> whitePieceType;
    private List<int> whitePieceMaterial;
    private List<int> whitePieceStartingX;
    private List<int> whitePieceStartingY;

    private List<int> blackPieceType;
    private List<int> blackPieceMaterial;
    private List<int> blackPieceStartingX;
    private List<int> blackPieceStartingY;

    public static bool amPlaying = false;

    private void Awake()
    {
        ResetTurn();

        GenerateAllTiles(tileSize, tileCountX, tileCountY);

    }

    public void LoadData(GameData data)
    {
        this.blackTeamMaxWidth = data.blackTeamMaxWidth;
        this.whiteTeamMaxWidth = data.whiteTeamMaxWidth;

        this.whitePieceType = data.whitePieceType;
        this.whitePieceMaterial = data.whitePieceMaterial;
        this.whitePieceStartingX = data.whitePieceStartingX;
        this.whitePieceStartingY = data.whitePieceStartingY;

        this.blackPieceType = data.blackPieceType;
        this.blackPieceMaterial = data.blackPieceMaterial;
        this.blackPieceStartingX = data.blackPieceStartingX;
        this.blackPieceStartingY = data.blackPieceStartingY;
        Debug.Log("Loaded!");
        StartGame();
    }

    public void SaveData(GameData data)
    {
        Debug.Log("Hi");
        data.lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        Debug.Log(data.lastPlayed = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

        data.blackTeamMaxWidth = this.blackTeamMaxWidth;
        data.whiteTeamMaxWidth = this.whiteTeamMaxWidth;

        data.whitePieceType = this.whitePieceType;
        data.whitePieceMaterial = this.whitePieceMaterial;
        data.whitePieceStartingX = this.whitePieceStartingX;
        data.whitePieceStartingY = this.whitePieceStartingY;

        data.blackPieceType = this.blackPieceType;
        data.blackPieceMaterial = this.blackPieceMaterial;
        data.blackPieceStartingX = this.blackPieceStartingX;
        data.blackPieceStartingY = this.blackPieceStartingY;
        Debug.Log("Saved!");
    }

    private void Start()
    {
        bgMusic = GameObject.Find("Background Music").GetComponent<MusicController>();
        if (bgMusic.GetComponent<AudioSource>().isPlaying == false)
            bgMusic.GetComponent<AudioSource>().Play();
        bgMusic.RegisterSoundControl(toggleSound);
    }

    private void StartGame()
    {
        whitePieceActualIndex = new List<int>();
        blackPieceActualIndex = new List<int>();

        SpawnAllPieces();
        PositionAllPieces();
        amPlaying = true;
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen();
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Chessboard Tile", "Chessboard Tile Hover", "Chessboard Tile Highlight")) && amPlaying == true)
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

                    Debug.Log(chessPieces[hitPosition.x, hitPosition.y].index);
                    // Is it our turn?
                    if ((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn) || (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn))
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];

                        currentlyDragging.PlayPickupSound();

                        // Get a list of where I can go, highlight tiles
                        availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, tileCountX, tileCountY);
                        // Get a list of special moves
                        specialMove = currentlyDragging.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves, new Vector2Int(tileCountX,tileCountY));

                        //PreventCheck();
                        
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
                {

                    currentlyDragging.PlayPickupSound();
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                }

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

    // Spawning of the whiteTeamPieces
    private void SpawnAllPieces()
    {
        chessPieces = new ChessPiece[tileCountX, tileCountY];

        int teamWidth = 8;

        int startingX = (tileCountX - teamWidth) / 2;

        // White Team
        for (int i = 0; i < whitePieceType.Count; i++)
        {
            whitePieceActualIndex.Add(i);
            chessPieces[startingX + whitePieceStartingX[i], whitePieceStartingY[i]] = SpawnSinglePiece(i, (ChessPieceType)whitePieceType[i], 0, whitePieceMaterial[i]);
        }

        // Black Team
        for (int i = 0; i < blackPieceType.Count; i++)
        {
            blackPieceActualIndex.Add(i);
            chessPieces[(tileCountX-1)-(startingX + blackPieceStartingX[i]), (tileCountY-1) - blackPieceStartingY[i]] = SpawnSinglePiece(i, (ChessPieceType)blackPieceType[i], 1, blackPieceMaterial[i]);
        }
    }
    private ChessPiece SpawnSinglePiece(int index, ChessPieceType type, int team, int material = 0)
    {
        ChessPiece piece = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

        piece.index = index;
        piece.type = type;
        piece.team = team;
        piece.material = (ChessPieceMaterial) material;

        Material[] teamMaterial;

        switch (piece.material)
        {
            case ChessPieceMaterial.Glass:
                teamMaterial = piece.team == 0 ? glassWhiteMaterials : glassBlackMaterials;
                break;
            case ChessPieceMaterial.Ceramic:
                teamMaterial = piece.team == 0 ? ceramicWhiteMaterials : ceramicBlackMaterials;
                break;
            case ChessPieceMaterial.Stone:
                teamMaterial = piece.team == 0 ? stoneWhiteMaterials : stoneBlackMaterials;
                break;
            case ChessPieceMaterial.Diamond:
                teamMaterial = piece.team == 0 ? diamondWhiteMaterials : diamondBlackMaterials;
                break;
            default:
                teamMaterial = piece.team == 0 ? basicWhiteMaterials : basicBlackMaterials;
                break;
        }

        piece.GetComponent<MeshRenderer>().material = piece.type switch
        {
            ChessPieceType.Pawn => teamMaterial[1],
            ChessPieceType.Rook => teamMaterial[2],
            ChessPieceType.Knight => teamMaterial[3],
            ChessPieceType.Bishop => teamMaterial[4],
            ChessPieceType.Queen => teamMaterial[5],
            ChessPieceType.King => teamMaterial[6],
            _ => teamMaterial[0]
        };

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

    private void Stalemate()
    {
        DisplayVictory(2);
    }

    private void DisplayVictory(int winningTeam)
    {
        amPlaying = false;
        victoryScreen.SetActive(true);
        victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
    }

    public void OnResetButton()
    {
        // UI
        victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(1).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(2).gameObject.SetActive(false);
        victoryScreen.SetActive(false);
        pauseScreen.SetActive(false);
        checkNotif.transform.GetChild(0).gameObject.SetActive(false);
        checkNotif.transform.GetChild(1).gameObject.SetActive(false);

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
        if(bgMusic.GetComponent<AudioSource>().isPlaying == false)
            bgMusic.GetComponent<AudioSource>().Play();
        StartGame();
    }

    public void OnExitButton()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(0);
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
                    myPawn.PlayTakePieceSound();
                    if (enemyPawn.team == 0)
                    {
                        deadWhitePieces.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                            new Vector3(tileCountX * tileSize, yOffset, -1 * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0, tileSize / 2)
                            + (Vector3.forward * deathSpacing) * deadWhitePieces.Count);
                    }
                    else
                    {
                        deadBlackPieces.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                            new Vector3(-1 * tileSize, yOffset, tileCountX * tileSize)
                            - bounds
                            + new Vector3(tileSize / 2, 0, tileSize / 2)
                            + (Vector3.back * deathSpacing) * deadBlackPieces.Count);
                    }
                    chessPieces[enemyPawn.currentX, enemyPawn.currentY] = null;
                    RemovePieceFromTeam(enemyPawn);
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
                    ChessPiece newQueen = SpawnSinglePiece(targetPawn.index, ChessPieceType.Queen, 0);
                    newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
                    Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
                    chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
                    PositionSinglePiece(lastMove[1].x, lastMove[1].y);
                }
                if (targetPawn.team == 1 && lastMove[1].y == 0)
                {
                    ChessPiece newQueen = SpawnSinglePiece(targetPawn.index, ChessPieceType.Queen, 1);
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

    private void DisplayCheck()
    {
        // Disable both check notifications
        checkNotif.transform.GetChild(0).gameObject.SetActive(false);
        checkNotif.transform.GetChild(1).gameObject.SetActive(false);

        // For every tile
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                // If the tile has a piece
                if (chessPieces[x, y] != null)
                {
                    // For each move the piece can do
                    foreach (var move in chessPieces[x,y].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY))
                    {
                        // If the tile the piece can move to is a king of the opposite team
                        if (chessPieces[move.x, move.y] != null)
                        {
                            if (chessPieces[move.x, move.y].type == ChessPieceType.King &&
                                chessPieces[move.x, move.y].team != chessPieces[x,y].team)
                            {
                                // Set the piece to having the king in check
                                Debug.Log(chessPieces[x,y].type + " at (" + x + ", " + y + ") has the enemy in check!");
                                chessPieces[x,y].hasKingInCheck = true;
                            }
                            else
                            {
                                // Else remove that status
                                chessPieces[x,y].hasKingInCheck = false;
                            }
                        }
                    }

                    // If the piece has the enemy king in check, tell us
                    if (chessPieces[x, y].hasKingInCheck == true)
                    {
                        checkNotif.transform.GetChild(chessPieces[x, y].team == 0 ? 1 : 0).gameObject.SetActive(true);
                    }
                }
            }
            
        }
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

            // Get all the simulated attacking whiteTeamPieces moves

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
                        {
                            targetKing = chessPieces[x, y];
                        }
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

        if (targetKing == null)
        {
            if (victoryScreen.activeInHierarchy)
            {
                Debug.Log("Checkmate");
            }
            else
            {
                Debug.LogError("No king was found and there hasn't been a checkmate");
            }
            return false;
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

    private bool CheckforStalemate()
    {
        // Set the team to whoever's turn it just became
        int targetTeam = isWhiteTurn ? 0 : 1;
        // A list of moves that whiteTeamPieces on the team can move to
        List<Vector2Int> piecesWithMoves = new List<Vector2Int>();
        // A list for moves that the attacking team can move to, that need to be removed
        List<Vector2Int> movesToRemove = new List<Vector2Int>();

        // Get all the moves for all the whiteTeamPieces on the team
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (chessPieces[x,y] != null)
                    if (chessPieces[x, y].team == targetTeam)
                        if(chessPieces[x, y].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY).Count > 0)
                        {

                            foreach (var move in chessPieces[x, y].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY))
                            {
                                piecesWithMoves.Add(move);
                            }
                        }
            }
            
            
        }

        // Add the moves that the attacking team are blocking to the removal list
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (chessPieces[x, y] != null)
                    if (chessPieces[x, y].team != targetTeam)
                        if (chessPieces[x, y].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY).Count > 0)
                        {

                            foreach (var move in chessPieces[x, y].GetAvailableMoves(ref chessPieces, tileCountX, tileCountY))
                            {
                                foreach (var spot in piecesWithMoves)
                                {
                                    if (move == spot)
                                    {
                                        movesToRemove.Add(spot);
                                    }
                                }
                            }
                        }
            }
        }

        // Remove the moves from the removal list
        for (int i = 0; i < movesToRemove.Count; i++)
        {
            piecesWithMoves.Remove(movesToRemove[i]);
        }

        // If the list is empty, no one on the team can move, so return stalemate
        if (piecesWithMoves.Count <= 0)
            return true;


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

            piece.PlayTakePieceSound();
            

                // If it's the enemy team
            if (otherPiece.team == 0)
            {
                if (otherPiece.type == ChessPieceType.King)
                    Checkmate(1);

                deadWhitePieces.Add(otherPiece);
                otherPiece.SetScale(Vector3.one * deathSize);
                otherPiece.SetPosition(
                    new Vector3(tileCountX * tileSize, yOffset, -1 * tileSize)
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
                    new Vector3(-1 * tileSize, yOffset, tileCountX * tileSize)
                    - bounds
                    + new Vector3(tileSize / 2, 0, tileSize / 2)
                    + (Vector3.back * deathSpacing) * deadBlackPieces.Count);
            }
            RemovePieceFromTeam(otherPiece);
        }
        else
        {
            piece.PlayMoveSound();
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

        if (CheckforStalemate())
        {
            Stalemate();
        }

        DisplayCheck();

        Debug.Log("Moved from " + moveList[moveList.Count - 1][0] + " to " + moveList[moveList.Count - 1][1]);

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

    public void TogglePauseScreen()
    {
        if(victoryScreen.activeInHierarchy == false)
        {
            pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
            if (pauseScreen.activeInHierarchy == true)
                bgMusic.GetComponent<AudioSource>().Pause();
            else
                bgMusic.GetComponent<AudioSource>().Play();
            amPlaying = !amPlaying;
        }
    }

    public void RemovePieceFromTeam(ChessPiece piece)
    {
        if (piece.team == 0)
        {
            int actualPieceIndex = whitePieceActualIndex.IndexOf(piece.index);
            if (piece.type != ChessPieceType.King)
            {
                whitePieceType.RemoveAt(actualPieceIndex);
                whitePieceMaterial.RemoveAt(actualPieceIndex);
                whitePieceStartingX.RemoveAt(actualPieceIndex);
                whitePieceStartingY.RemoveAt(actualPieceIndex);
                whitePieceActualIndex.RemoveAt(actualPieceIndex);
            }
        }
        else if(piece.team == 1)
        {
            if (piece.type != ChessPieceType.King)
            {
                int actualPieceIndex = blackPieceActualIndex.IndexOf(piece.index);
                blackPieceType.RemoveAt(actualPieceIndex);
                blackPieceMaterial.RemoveAt(actualPieceIndex);
                blackPieceStartingX.RemoveAt(actualPieceIndex);
                blackPieceStartingY.RemoveAt(actualPieceIndex);
                blackPieceActualIndex.RemoveAt(actualPieceIndex);
            }
        }
    }
}
