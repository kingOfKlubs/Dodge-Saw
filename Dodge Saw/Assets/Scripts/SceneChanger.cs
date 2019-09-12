using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneChanger : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject _gameOverUI;

    public void StartGame()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
        Debug.Log(volume);
    }

    public void Update()
    {
        if(Movement.Death == true)
        {
            
                _gameOverUI.SetActive(true);
            
        }
            else
                _gameOverUI.SetActive(false);
    }
}
