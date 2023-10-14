using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The Script That "Loads" the city: Waits 3 Seconds Before Switching To City Scene.
public class Loading : MonoBehaviour
{
    /// <summary>
    /// A function that runs at the programs' start.
    /// </summary>
    private void Awake()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// A function that called before the first frame update
    /// </summary>
    void Start()
    {
        StartCoroutine(Load());
    }

    /// <summary>
    /// An IEnumerator function that loads the city scene.
    /// </summary>
    IEnumerator Load()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(3);

    }


}
