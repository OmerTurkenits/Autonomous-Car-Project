using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stores The Settings Values.
public class NNetValues : MonoBehaviour
{
    #region ======================   NNet Settings Values  =======================
    public static int HiddenLayerCount = 1;
    public static int HiddenNeuronCount = 10;
    public static float DistanceMultiplier = 1.4f;
    public static float AverageSpeedMultiplier = 0.2f;
    public static float SensorMultiplier = 0.1f;
    public static float MutationRate = 0.055f;
    #endregion

    private static GameObject instance;

    /// <summary>
    /// A function that is called when object is loaded
    /// "doesn't destroy the object when loading into the city thus allowing to save the NNet settings values."
    /// </summary>
    /// <param></param>
    /// <param></param>
    /// <returns></returns>
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(this.gameObject);
    }
}
