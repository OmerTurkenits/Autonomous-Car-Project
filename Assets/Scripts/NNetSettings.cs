using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NNetSettings : MonoBehaviour
{
    #region ======================   Settings UI Elements  =======================
    public TMP_InputField HiddenLayerCountInput;

    public TMP_InputField HiddenNeuronCountInput;

    public Slider DistanceMultiplierInput;
    public TextMeshProUGUI DistanceMultiplierText;

    public Slider AverageSpeedMultiplierInput;
    public TextMeshProUGUI AverageSpeedMultiplierText;

    public Slider SensorMultiplierInput;
    public TextMeshProUGUI SensorMultiplierText;

    public Slider MutationRateInput;
    public TextMeshProUGUI MutationRateText;
    #endregion

    //Allows to stop the update when the reset button is pressed in order to reset values
    private bool isResetting = false; 

    // Start is called before the first frame update
    void Start()
    {
        #region ======================   Sets The Settings UI Elements Starting Values  =======================
        HiddenLayerCountInput.text = NNetValues.HiddenLayerCount.ToString();
        HiddenNeuronCountInput.text = NNetValues.HiddenNeuronCount.ToString();

        DistanceMultiplierInput.maxValue = 10;
        DistanceMultiplierInput.minValue = 0;

        DistanceMultiplierInput.value = NNetValues.DistanceMultiplier;

        AverageSpeedMultiplierInput.maxValue = 10;
        AverageSpeedMultiplierInput.minValue = 0;

        AverageSpeedMultiplierInput.value = NNetValues.AverageSpeedMultiplier;

        SensorMultiplierInput.maxValue = 10;
        SensorMultiplierInput.minValue = 0;

        SensorMultiplierInput.value = NNetValues.SensorMultiplier;

        MutationRateInput.maxValue = 1;
        MutationRateInput.minValue = 0;

        MutationRateInput.value = NNetValues.MutationRate;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region ==============  Change NNetValues Variables To The UI Elements Current Values Every Frame  ===============
        if (!isResetting) //If Setting Not Resetting 
        {
            NNetValues.HiddenLayerCount = int.Parse(HiddenLayerCountInput.text);
            NNetValues.HiddenNeuronCount = int.Parse(HiddenNeuronCountInput.text);
            NNetValues.DistanceMultiplier = DistanceMultiplierInput.value = Mathf.Round(DistanceMultiplierInput.value * 100f) / 100f;
            NNetValues.AverageSpeedMultiplier = AverageSpeedMultiplierInput.value = Mathf.Round(AverageSpeedMultiplierInput.value * 100f) / 100f;
            NNetValues.SensorMultiplier = SensorMultiplierInput.value = Mathf.Round(SensorMultiplierInput.value * 100f) / 100f;
            NNetValues.MutationRate = MutationRateInput.value = Mathf.Round(MutationRateInput.value * 1000f) / 1000f;
        }
        #endregion

        DistanceMultiplierText.text = DistanceMultiplierInput.value+"";
        AverageSpeedMultiplierText.text = AverageSpeedMultiplierInput.value + "";
        SensorMultiplierText.text = SensorMultiplierInput.value + "";
        MutationRateText.text = MutationRateInput.value + "";

    }


    public void resetValues()
    {

        isResetting = true;

        NNetValues.HiddenLayerCount = 1;
        NNetValues.HiddenNeuronCount = 10;
        NNetValues.DistanceMultiplier = 1.4f;
        NNetValues.AverageSpeedMultiplier = 0.2f;
        NNetValues.SensorMultiplier = 0.1f;
        NNetValues.MutationRate = 0.055f;

        HiddenLayerCountInput.text = NNetValues.HiddenLayerCount.ToString();
        HiddenNeuronCountInput.text = NNetValues.HiddenNeuronCount.ToString();
        DistanceMultiplierInput.value = NNetValues.DistanceMultiplier;
        AverageSpeedMultiplierInput.value = NNetValues.AverageSpeedMultiplier;
        SensorMultiplierInput.value = NNetValues.SensorMultiplier;
        MutationRateInput.value = NNetValues.MutationRate;

        isResetting = false;
    
    }

}
