using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public TileTheme tileTheme;

    public int numberOfBombs = 5;
    public int numberOfRows = 10;
    public int numberOfColumns = 10;

    public EndMenu endGameMenu;
    public Board gameBoard;

    public bool isPlaying { get; private set; }

	void Awake ()
    {
        isPlaying = false;

        InitializeSingleton();
        ValidateTileTheme();
    }

    void Start()
    {
        BeginGame();
    }

    void InitializeSingleton()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than one Game Manager Instance");
            Destroy(instance);
        }

        instance = this;
    }

    void ValidateTileTheme()
    {
        if (tileTheme == null)
        {
            throw new System.Exception("Missing tile theme");
        }
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
