using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMenu : MonoBehaviour
{

    [SerializeField]
    Text text;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void ValueChanged()
    {
        text.text = slider.value.ToString();

        if (text.name == "nRows")
            GameController.Instance.rows = (int)slider.value;

        else if (text.name == "nColumns")
            GameController.Instance.columns = (int)slider.value;
    }
}
