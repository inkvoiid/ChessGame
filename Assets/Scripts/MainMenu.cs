using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickPlay()
    {

    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickScene2()
    {
        SceneManager.LoadScene(2);
    }
}
