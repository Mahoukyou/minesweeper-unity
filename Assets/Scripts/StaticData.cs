using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData
{
    public static int minimumAmountOfRows = 3;
    public static int maximumAmountOfRows = 25;
    public static int minimumAmountOfColumns = 3;
    public static int maximumAmountOfColumns = 25;

    public static int minimumAmountOfBombs = 1;

    public static int GetMaximumAmountOfBombs(int rows, int columns)
    {
        return rows * columns - 1;
    }
}
