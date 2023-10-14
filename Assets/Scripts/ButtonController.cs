using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class ButtonController : MonoBehaviour
{

    public Button pauseButton;
    public Button continueButton;



    public void MenuToLoading()
    {
        SceneManager.LoadScene(2); // loads the first scene in the builder - (from the menu to the city). 
    }

    public void MenuToSettings()
    {
        SceneManager.LoadScene(1); // loads the first scene in the builder - (from the menu to the city). 
    }

    public void SettingToMenu()
    {
        SceneManager.LoadScene(0); // loads the first scene in the builder - (from the menu to the city). 
    }


    public void DriveToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }


    public void PauseButton()
    {
        pauseButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        pauseButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }
    public void QuitButton()
    {
        Application.Quit();
    }


 
}