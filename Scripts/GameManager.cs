using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameover = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _gameover == true)
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(1); //Scene 1 = Current Game Scene
    }
    public void SetGameOver()
    {
        _gameover = true;
    }
}
