using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSizeSliderManager : MonoBehaviour
{
    public Slider rowSlider;
    public Slider columnSlider;
    public Slider bombsSlider;

    public int defaultBoardSize = 5;
    public int defaultAmountOfBombs = 5;

    void Start()
    {
        SetSlidersBoundries();
        SetDefaultValues();
        UpdateBombsSlider();
    }

    void SetSlidersBoundries()
    {
        rowSlider.minValue = StaticData.minimumAmountOfRows;
        rowSlider.maxValue = StaticData.maximumAmountOfRows;
        columnSlider.minValue = StaticData.minimumAmountOfColumns;
        columnSlider.maxValue = StaticData.maximumAmountOfColumns;

        bombsSlider.minValue = StaticData.minimumAmountOfBombs;

        rowSlider.wholeNumbers = true;
        columnSlider.wholeNumbers = true;
        bombsSlider.wholeNumbers = true;
    }

    void SetDefaultValues()
    {
        rowSlider.value = Mathf.Clamp(defaultBoardSize, rowSlider.minValue, rowSlider.maxValue);
        columnSlider.value = Mathf.Clamp(defaultBoardSize, columnSlider.minValue, columnSlider.maxValue);
    }

    public void UpdateBombsSlider()
    {
        bombsSlider.maxValue = StaticData.GetMaximumAmountOfBombs((int)rowSlider.value, (int)columnSlider.value);
        bombsSlider.value = Mathf.Clamp(bombsSlider.value, bombsSlider.minValue, bombsSlider.maxValue);
    }

}
	
