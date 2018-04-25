using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public TileTheme tileTheme;

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
        int[] dim = new int[] { GameSettings.amountOfRows, GameSettings.amountOfColumns };
        gameBoard.InitializeBoard(GameSettings.amountOfBombs, dim);

        isPlaying = true;
    }

    public void GameLost()
    {
        isPlaying = false;
        endGameMenu.ToggleMenu(true);
    }

    public void GameWon()
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
