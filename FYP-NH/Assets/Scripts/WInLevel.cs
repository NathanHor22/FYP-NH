using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinLevel : MonoBehaviour
{
    public string scenename;
    public void PlayGame()
    {
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame ()
    {
        //to check whether this method works
        Debug.Log("Quit");
        Application.Quit();
    }
}
