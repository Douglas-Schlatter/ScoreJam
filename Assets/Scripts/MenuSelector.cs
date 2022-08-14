using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Instructions()
    {
        SceneManager.LoadScene(2);
    }

    public void Options()
    {
        SceneManager.LoadScene(3);
    }

    public void ScoreBoard()
    {
        SceneManager.LoadScene(4);
    }

    public void Credits()
    {
        SceneManager.LoadScene(5);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
