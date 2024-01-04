using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(SceneName.IntroStory);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
