using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private float rotationX = 60.0f;
    private float rotationY = 0.0f;

    private bool isTopDown = false;
    private Vector3 lastPos = Vector3.zero;

    private void Start()
    {
        transform.eulerAngles = new Vector3(60, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(1) && Chessboard.currentlyDragging == null && !isTopDown && Chessboard.amPlaying == true)
        {
            rotationX = speed * -Input.GetAxis("Mouse Y");
            rotationY = speed * Input.GetAxis("Mouse X");

            rotationX = Mathf.Clamp(transform.eulerAngles.x + rotationX, -90f, 90f);
            rotationY = transform.eulerAngles.y + rotationY;

            transform.eulerAngles = new Vector3(rotationX, rotationY, 0);

        }

        // Zoom function not fully implemented

        //if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0 && Chessboard.currentlyDragging == null)
        //{
        //    Chessboard.GetCurrentCamera().fieldOfView -= Input.mouseScrollDelta.y + Time.deltaTime * 10;
        //}
    }

    public void OnTopDownButton()
    {
        if(isTopDown)
        {
            isTopDown = false;
            transform.eulerAngles = lastPos;
            GameObject.Find("ToggleTopDown").GetComponentInChildren<TextMeshProUGUI>().text = "Top Down View";
        }
        else
        {
            isTopDown = true;
            lastPos = transform.eulerAngles;
            transform.eulerAngles = new Vector3(90, 0, 0);
            GameObject.Find("ToggleTopDown").GetComponentInChildren<TextMeshProUGUI>().text = "Perspective View";
        }
    }
}
