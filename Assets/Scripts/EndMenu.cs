using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void SetScore(int score)
    {

    }

    public void ToggleMenu(bool show)
    {
        gameObject.SetActive(show);
    }

    public void PlayAgain()
    {
        GameManager.instance.RestartGame();
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
