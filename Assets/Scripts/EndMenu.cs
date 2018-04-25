using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
}
