using System;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Prefabs and Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;

    [SerializeField] private Material[] basicWhiteMaterials;
    [SerializeField] private Material[] basicBlackMaterials;

    // Logic
    private ChessPiece[,] chessPieces;
    private ChessPiece currentlyDragging;
    private List<ChessPiece> deadWhitePieces = new List<ChessPiece>();
    private List<ChessPiece> deadBlackPieces = new List<ChessPiece>();
    private const int tileCountX = 8;
    private const int tileCountY = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;

    private void Awake()
    {
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
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Chessboard Tile", "Chessboard Tile Hover")))
        {
            // Gets the indices of the chessboard tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            // If we're hovering a tile after not hovering any tiles
            if(currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Chessboard Tile Hover");
            }

            // If we were alreay hovering a tile, change the previous one
            if(currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Chessboard Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Chessboard Tile Hover");
            }

            // If we press down on the mouse
            if (Input.GetMouseButtonDown(0))
            {
                if (chessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    // Is it our turn?
                    if (true)
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];
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
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    currentlyDragging = null;
                }
                else
                {
                    currentlyDragging = null;
                }
            }
        }
        else
        {
            if(currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Chessboard Tile");
                currentHover = -Vector2Int.one;
            }

            if (currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
            }
        }

        // If we're dragging a piece
        if (currentlyDragging)
        {
            Plane horizonPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if(horizonPlane.Raycast(ray, out distance))
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
        GameObject tileObject = new GameObject(x+ ", " + y);
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds;
        vertices[2] = new Vector3((x+1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x+1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

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

        // WhiteTeam
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, basicWhite);
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, basicWhite);
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, basicWhite);
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, basicWhite);
        chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, basicWhite);
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, basicWhite);
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, basicWhite);
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, basicWhite);
        for (int i = 0; i < tileCountX; i++)
        {
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, basicWhite);
        }

        // BlackTeam
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, basicBlack);
        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, basicBlack);
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, basicBlack);
        chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, basicBlack);
        chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, basicBlack);
        chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, basicBlack);
        chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, basicBlack);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, basicBlack);
        for (int i = 0; i < tileCountX; i++)
        {
            chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, basicBlack);
        }
    }
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece piece = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

        piece.type = type;
        piece.team = team;

        Material[] teamMaterial;

        if(piece.team == 0)
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
                if(chessPieces[x,y] != null)
                {
                    PositionSinglePiece(x, y, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        chessPieces[x,y].currentX = x;
        chessPieces[x,y].currentY = y;
        chessPieces[x,y].SetPosition(GetTileCenter(x, y), force);
    }
    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }

    // Operations
    private bool MoveTo(ChessPiece piece, int x, int y)
    {
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
}
