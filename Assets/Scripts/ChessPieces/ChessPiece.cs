using System;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public enum ChessPieceMaterial
{
    Basic = 0,
    Glass = 1,
    Ceramic = 2,
    Stone = 3,
    Metal = 4,
    Diamond = 5
}

    public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public Vector2Int startingPos;
    public int index;
    public ChessPieceType type;
    public ChessPieceMaterial material;
    public bool hasKingInCheck = false;

    public string abilities = "";

    // Abilities
    public bool abilitySidestep = false;
    public bool abilityBackpedal = false;

    private Vector3 desiredPosition;
    private Vector3 desiredScale = Vector3.one;

    public static List<Vector2Int> allWhiteRookStartingPos;
    public static List<Vector2Int> allBlackRookStartingPos;

    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip takePieceSound;
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;

    private void Start()
    {
        transform.rotation = Quaternion.Euler((team == 1) ? Vector3.zero : new Vector3(0, 180, 0));
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);            // Lerp is linear interpolate (smoothly moves between two places.)
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    //public ChessPieceData SaveChessPieceData()
    //{
    //    var pieceData = new ChessPieceData();
    //    pieceData.type = this.type;
    //    pieceData.material = this.material;
    //    pieceData.team = this.team;

    //    return pieceData;
    //}

    //public void RestoreFromData(ChessPieceData data)
    //{
    //    if (data == null) Debug.LogError("Couldn't restore chess piece data from file as it was null.");

    //    this.type = data.type;
    //    this.material = data.material;
    //    this.team = data.team;
    //}

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        // Don't modify any board code in here

        List<Vector2Int> r = new List<Vector2Int>();

        //Possibly add logic for walls in here so you can't move to them
        // (Check if wall before move or something)
        r.Add(new Vector2Int(3, 3));
        r.Add(new Vector2Int(3, 4));
        r.Add(new Vector2Int(4, 3));
        r.Add(new Vector2Int(4, 4));

        return r;
    }

    public virtual SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
    {
        return SpecialMove.None;
    }

    public virtual SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves, Vector2Int boardDimensions)
    {
        return SpecialMove.None;
    }

    public virtual void SetPosition(Vector3 position, bool force = false)           // Virtual allows the Method to be overriden in derived classes using override - TIL (11/8/22)
    {
        desiredPosition = position;
        if (force)
        {
            transform.position = desiredPosition;
        }
    }

    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        desiredScale = scale;
        if (force)
        {
            transform.localScale = desiredScale;
        }
    }

    public virtual void PlayPickupSound()
    {
        audioSource.clip = pickupSound;
        audioSource.Play();
    }

    public virtual void PlayMoveSound()
    {
        audioSource.clip = moveSound;
        audioSource.Play();
    }

    public virtual void PlayTakePieceSound()
    {
        audioSource.clip = takePieceSound;
        audioSource.Play();
    }
}

[System.Serializable]
public class ChessPieceData
{
    public ChessPieceType type;
    public ChessPieceMaterial material;
    public int team;
}
