using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Handler : MonoBehaviour
{
    public enum UI_State
    {
        Playing, PauseMenu, MainMenu, OptionMenu, GameOver
    };

    public UI_State CurrentState = UI_State.MainMenu;

    public GameObject Background;
    public GameObject MainMenu;
    public GameObject OptionMenu;
    public GameObject PauseMenu;
    public GameObject GameOverUI;

    void Awake()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case UI_State.Playing:
                if (Input.GetKeyDown(KeyCode.Escape))
                    Pause();
                break;
        }
        
    }

    public void SwitchUIState(UI_State newState)
    {
        switch (CurrentState)
        {
            case UI_State.Playing:
                Background.SetActive(true);
                break;
            case UI_State.PauseMenu:
                PauseMenu.SetActive(false);
                break;
            case UI_State.MainMenu:
                MainMenu.SetActive(false);
                break;
            case UI_State.OptionMenu:
                OptionMenu.SetActive(false);
                break;
            case UI_State.GameOver:
                GameOverUI.SetActive(false);
                break;
        }

        switch (newState)
        {
            case UI_State.Playing:
                Background.SetActive(false);
                Time.timeScale = 1.0f;
                break;
            case UI_State.PauseMenu:
                Time.timeScale = 0f;
                PauseMenu.SetActive(true);
                break;
            case UI_State.MainMenu:
                Time.timeScale = 0f;
                MainMenu.SetActive(true);
                break;
            case UI_State.OptionMenu:
                Time.timeScale = 0f;
                OptionMenu.SetActive(true);
                break;
            case UI_State.GameOver:
                Time.timeScale = 0f;
                GameOverUI.SetActive(true);
                break;
        }

        CurrentState = newState;
    }

    public void Pause()
    {
        SwitchUIState(UI_State.PauseMenu);
    }

    public void Play()
    {
        SwitchUIState(UI_State.Playing);
    }

    public void Option()
    {
        SwitchUIState(UI_State.OptionMenu);
    }

    public void StartGame()
    {
        SwitchUIState(UI_State.Playing);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SwitchUIState(UI_State.MainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Debug.Log("sssssss");
        SwitchUIState(UI_State.GameOver);
    }
}
