using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBoardSize : MonoBehaviour
{
    public BoardSizeSliderManager b;


    public void Play()
    {
        SetGameSettings();
        SceneManager.LoadScene("Main");   
    }

    void SetGameSettings()
    {
        GameSettings.amountOfRows = (int)b.rowSlider.value;
        GameSettings.amountOfColumns = (int)b.columnSlider.value;
        GameSettings.amountOfBombs = (int)b.bombsSlider.value;  
    }
}
