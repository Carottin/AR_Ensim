using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderView : MonoBehaviour {


    [SerializeField]
    Slider m_slider;

    public float FillTime = 3.0f;
    // Use this for initialization

    public void Reset()
    {
        m_slider.minValue = Time.time;
        m_slider.maxValue = Time.time + FillTime;
    }
    void Update()
    {
        m_slider.value = Time.time;
    }

}
