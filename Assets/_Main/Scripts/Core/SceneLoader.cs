using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button exitButton;
    public Button startButton;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
        startButton.onClick.AddListener(StartGame);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void StartGame()
    {
        SceneManager.LoadScene("VisualNovel");
    }
}
