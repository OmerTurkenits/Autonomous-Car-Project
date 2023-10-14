using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SpeedController : MonoBehaviour
{
    
    public TextMeshProUGUI speedText; // The text that shows the speed on screen
    public Slider speedSlider; // The slider that controls the speed


    // Start is called before the first frame update
    void Start()
    { 
        speedSlider.maxValue = 60;
        speedSlider.minValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeScale != 0)
        {
            Time.timeScale = speedSlider.value;
            speedText.text = "Simulation Speed: " + Math.Floor(speedSlider.value);
        }
        
    }



}
