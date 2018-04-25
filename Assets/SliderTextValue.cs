using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{
    public Slider slider;
    public Text text;

    private void Start()
    {
        OnSliderValueUpdate();
    }

    public void OnSliderValueUpdate()
    {
        text.text = slider.value.ToString();
    }
	
}
