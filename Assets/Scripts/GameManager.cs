using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject playerObject;
    PlayerController playerController;
    int score;
    [SerializeField] public int scoreToNextLevel;
    InputSystem pauseAction;

    private void Awake()
    {
        pauseAction = new InputSystem();
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Start()
    {
        pauseAction.UI.Pause.performed += context => TogglePause();
        playerController = playerObject.GetComponent<PlayerController>();
    }

    private void TogglePause()
    {
        if(isPaused)
        {
            Resume();
        }
        else if(!isPaused)
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 1f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    private void Update()
    {
        score = playerController.score;

        if (score >= scoreToNextLevel)
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                endUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                GetComponent<LevelManager>().NextLevel();
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
