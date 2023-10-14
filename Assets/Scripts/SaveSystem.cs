using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public static NNet network = null;
    public static bool isNetwork = false;
    private static GameObject instance;

    public Toggle preMade;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(this.gameObject);

        AllowPreMade();


        }

    private void AllowPreMade()
    {
        if (!isNetwork)
            preMade.gameObject.SetActive(false);
        else
            preMade.gameObject.SetActive(true);
    }
}
