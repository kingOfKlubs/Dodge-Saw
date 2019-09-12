using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    public static bool _isGamePaused = false;
    public GameObject _pauseMenuUI;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
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
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isGamePaused = true;
    }
}
