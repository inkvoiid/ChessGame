using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private float rotationX = 60.0f;
    private float rotationY = 0.0f;

    private void Start()
    {
        transform.eulerAngles = new Vector3(60, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !Chessboard.amMovingPiece && Chessboard.currentlyDragging == null)
        {
            rotationX = speed * -Input.GetAxis("Mouse Y");
            rotationY = speed * Input.GetAxis("Mouse X");

            transform.eulerAngles +=  new Vector3(rotationX, rotationY, 0);

        }
    }
}
