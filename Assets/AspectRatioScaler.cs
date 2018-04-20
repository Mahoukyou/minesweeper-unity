using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioScaler : MonoBehaviour
{
    float aspectRatio = 0.0f;

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
