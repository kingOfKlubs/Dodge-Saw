using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneChanger : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject _gameOverUI;
    [SerializeField]
    public string startGameName;
    public string mainMenuName;
    AudioManager AM;

    public void StartGame()
    {
        SceneManager.LoadScene(startGameName);
        AM = FindObjectOfType<AudioManager>();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
		FindObjectOfType<AudioManager>().Play("HipHop");
        SceneManager.LoadScene("Options");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
		FindObjectOfType<AudioManager>().Play("Theme");
        FindObjectOfType<AudioManager>().Stop("HipHop");
        SceneManager.LoadScene(mainMenuName);
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
        Debug.Log(volume);
    }

    public void Update()
    {
        if (_gameOverUI != null)
        {
            if (Movement.Death == true)
            {

                _gameOverUI.SetActive(true);

            }
            else
                _gameOverUI.SetActive(false);
        }
    }
}
