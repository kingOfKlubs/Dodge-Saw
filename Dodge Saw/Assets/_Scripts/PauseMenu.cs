using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    public static bool _isGamePaused = false;
    public GameObject _pauseMenuUI;
    public GameObject _optionsUI;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if(_isGamePaused)
        {
            Movement movement = FindObjectOfType<Movement>();
            if (movement != null)
                movement._coolDownImageLarge.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
            {
                Resume();
            }
            else
                Pause();
        }
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isGamePaused = false;

    }
    public void Pause()
    {
        if (Movement.Death)
        {
            Debug.Log("Player is dead. Do nothing");
        }
        else {
            _pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _isGamePaused = true;
            if(_optionsUI.activeSelf == true)
            {
                _optionsUI.SetActive(false);
            }
        }
    }
    public void Options()
    {
        _pauseMenuUI.SetActive(false);
        _optionsUI.SetActive(true);
    }
}
