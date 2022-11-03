using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(string message)
    {
        gameObject.SetActive(true);
        text.text = message;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        text.text = String.Empty;
    }
}
