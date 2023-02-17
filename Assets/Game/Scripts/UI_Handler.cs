using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public enum UI_State
    {
        Play, Pause
    };

    public UI_State CurrentState;

    public GameObject MenuCanvas;
    //void Awake()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case UI_State.Play:
                if (Input.GetKeyDown(KeyCode.Escape))
                    Pause();
                break;
        }
        
    }

    public void SwitchUIState(UI_State newState)
    {
        switch (CurrentState)
        {

        }

        switch (newState)
        {
            case UI_State.Play:
                MenuCanvas.SetActive(false);
                Time.timeScale = 1.0f;
                break;
            case UI_State.Pause:
                Time.timeScale = 0f;
                MenuCanvas.SetActive(true);
                break;
        }

        CurrentState = newState;
    }

    public void Pause()
    {
        SwitchUIState(UI_State.Pause);
    }

    public void Play()
    {
        SwitchUIState(UI_State.Play);
    }
}
