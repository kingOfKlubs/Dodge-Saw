using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    public static bool _isGamePaused = false;
    public GameObject _pauseMenuUI;
    public GameObject _pauseUI;
    public GameObject _optionsUI;

    AudioManager audioManager;

    Movement movement;

    private void Start()
    {
        Time.timeScale = 1;
        movement = FindObjectOfType<Movement>();
    }

    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if(_isGamePaused)
        {
            //if (movement != null)
                //movement._coolDownImageLarge.gameObject.SetActive(false);
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
        audioManager.Play("Theme");
        _isGamePaused = false;
        AudioManager.instance.Play("ButtonPressed");
    }
    public void Pause()
    {
        if (Movement.Death)
        {
            Debug.Log("Player is dead. Do nothing");
        }
        else {
            _pauseMenuUI.SetActive(true);
            _pauseUI.SetActive(true);
            Time.timeScale = 0f;
            audioManager.Mute("Theme");
            _isGamePaused = true;
            if(_optionsUI.activeSelf == true)
            {
                _optionsUI.SetActive(false);
            }
            AudioManager.instance.Play("ButtonPressed");
        }
    }
    public void Options()
    {
        _pauseUI.SetActive(false);
        _optionsUI.SetActive(true);
        AudioManager.instance.Play("ButtonPressed");
    }
}
