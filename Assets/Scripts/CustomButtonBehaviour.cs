using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Image border;

public void ButtonHovered()
    {
        if(this.gameObject.GetComponent<Button>().interactable)
            border.gameObject.SetActive(true);
    }

    public void ButtonNotHovered()
    {
        border.gameObject.SetActive(false);
    }
}
