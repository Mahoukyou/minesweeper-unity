using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioScaler : MonoBehaviour
{
    float aspectRatio = 0.0f;

    void Start()
    {
        int maxRowsOrCols = Mathf.Max(GameSettings.amountOfRows, GameSettings.amountOfColumns) + 1;
        int prefsize = 128; // tmp for now

        GetComponent<CanvasScaler>().referenceResolution = new Vector2(prefsize * maxRowsOrCols, prefsize * maxRowsOrCols);
    }

    void Update()
    {
        float newAspectRatio = (float)Screen.width / Screen.height;
        if (newAspectRatio != aspectRatio)
        {
            aspectRatio = newAspectRatio;

            GetComponent<CanvasScaler>().matchWidthOrHeight = aspectRatio > 1.0f ? 1 : 0;
        }

        Debug.Log(aspectRatio);
    }
}
