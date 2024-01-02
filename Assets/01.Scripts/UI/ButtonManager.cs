using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameSceneMove()
    {
        SceneManager.LoadScene(SceneName.Ingame);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
