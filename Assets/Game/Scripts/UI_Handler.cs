using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Handler : MonoBehaviour
{
    public enum UI_State
    {
        Playing, PauseMenu, MainMenu, OptionMenu, GameOver, GameFinish
    };

    public UI_State CurrentState = UI_State.MainMenu;

    public GameObject Background;
    public GameObject MainMenu;
    public GameObject OptionMenu;
    public GameObject PauseMenu;
    public GameObject GameOverUI;
    public GameObject GameFinishUI;
    public GameObject GameUIHandler;
    void Awake()
    {
        switch (CurrentState)
        {
            case UI_State.Playing:
                Time.timeScale = 1.0f;
                Background.SetActive(false);
                MainMenu.SetActive(false);
                OptionMenu.SetActive(false);
                PauseMenu.SetActive(false);
                GameOverUI.SetActive(false);
                GameFinishUI.SetActive(false);
                GameUIHandler.SetActive(true);
                break;
            case UI_State.MainMenu:
                Time.timeScale = 0f;
                Background.SetActive(true);
                MainMenu.SetActive(true);
                OptionMenu.SetActive(false);
                PauseMenu.SetActive(false);
                GameOverUI.SetActive(false);
                GameFinishUI.SetActive(false);
                GameUIHandler.SetActive(false);
                break;
            case UI_State.GameFinish:
                Time.timeScale = 0f;
                Background.SetActive(true);
                MainMenu.SetActive(false);
                OptionMenu.SetActive(false);
                PauseMenu.SetActive(false);
                GameOverUI.SetActive(false);
                GameFinishUI.SetActive(true);
                GameUIHandler.SetActive(false);
                break;
        }
        
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
                GameUIHandler.SetActive(false);
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
            case UI_State.GameFinish:
                GameFinishUI.SetActive(false);
                break;
        }

        switch (newState)
        {
            case UI_State.Playing:
                Background.SetActive(false);
                GameUIHandler.SetActive(true);
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
            case UI_State.GameFinish:
                Time.timeScale = 0f;
                GameFinishUI.SetActive(true);
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
        SceneManager.LoadScene(0);
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
        SwitchUIState(UI_State.GameOver);
    }

    public void GameFinish() 
    {
        SwitchUIState(UI_State.GameFinish);
    }
}
