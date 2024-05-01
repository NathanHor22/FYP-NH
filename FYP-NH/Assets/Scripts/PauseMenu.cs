using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // private PlayerInput playerInput;
    // private CharacterController controller;
    // private InputAction pauseAction;
    public AudioSource BgSound;
    public GameObject pauseMenuUI;
    public static bool GameIsPaused = false;
    // private void Start () {
    //     controller = GetComponent<CharacterController>();
    //     playerInput = GetComponent<PlayerInput>();
    //     pauseAction = playerInput.actions["Pause"];
    // }

    void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    void Resume() 
    {
        BgSound.enabled = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause() 
    {
        BgSound.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

}
