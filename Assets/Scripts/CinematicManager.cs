using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    
    public Camera cinematic;
    public Camera a;
    public Camera b;
    public GameObject menuUI;

    [SerializeField] private float translateSpeed;

    private bool camStop = false;
    private bool isCamA = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        translateSpeed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camStop)
        {
            moveCam();
        }

        //toggleText();
    }


    public void moveCam() // Moves the camera up until y = 50, then reveals the UI
    {

        cinematic.transform.position += cinematic.transform.up * translateSpeed * 1;

            if (cinematic.transform.position.y > 50)
                translateSpeed -= 0.005f;


            if (translateSpeed < 0)
            {
            
                menuUI.gameObject.SetActive(true);
                translateSpeed = 0;
                camStop = true;

            }
    }

    public void ChangeCam()
    {
        isCamA = !isCamA;

        if (isCamA)
        {
            a.gameObject.SetActive(!a.enabled);
            b.gameObject.SetActive(b.enabled);
        }

        else
        {
            a.gameObject.SetActive(a.enabled);
            b.gameObject.SetActive(!b.enabled);
        }

    }

}
