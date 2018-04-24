using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int numberOfBombs = 5;
    public int numberOfRows = 10;
    public int numberOfColumns = 10;

    public EndMenu endGameMenu;
    public Board gameBoard;

    public bool isPlaying { get; private set; }

	void Awake ()
    {
        isPlaying = false;

        if(instance != null && instance != this)
        {
            Debug.LogWarning("More than one gamemanager instance");
            Destroy(instance);
        }

        instance = this;
	}

    void Start()
    {
        BeginGame();
    }

    void BeginGame()
    {
        int[] dim = new int[] { numberOfRows, numberOfColumns };
        gameBoard.InitializeBoard(numberOfBombs, dim);

        isPlaying = true;
    }

    public void GameLost()
    {
        isPlaying = false;
        endGameMenu.ToggleMenu(true);
    }

    public void RestartGame()
    {
        Debug.Log("restarting game");
        BeginGame();
        endGameMenu.ToggleMenu(false);
    }
}
