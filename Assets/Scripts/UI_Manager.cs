using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    
    public GameObject menuUI;
    public GameObject driveUI;
    public Camera cinematic;
    public GameObject genomeManager;

    // Start is called before the first frame update
    void Start()
    {
        menuUI.gameObject.SetActive(false); // Turns the menu UI off
        driveUI.gameObject.SetActive(false);

    }

    public void onSelect() // When button is pressed
    {

        Time.timeScale = 1;
        driveUI.gameObject.SetActive(true);
        genomeManager.gameObject.SetActive(true);
        menuUI.gameObject.SetActive(false);
        cinematic.gameObject.SetActive(false);
    }

}
