using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneChanger : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public GameObject _gameOverUI;
    public GameObject _gameOverRewardUI;
    [SerializeField]
    public string startGameName;
    public string mainMenuName;
    public Text rewardNumber;
    AudioManager AM;

    public void StartGame()
    {
        SceneManager.LoadScene(startGameName);
        AM = FindObjectOfType<AudioManager>();
        //slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        if(slider != null)
        slider.value = PlayerPrefs.GetFloat("VolumeA",0);
     
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
    public void Store()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
        FindObjectOfType<AudioManager>().Play("HipHop");
        SceneManager.LoadScene("Store");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuName);
		FindObjectOfType<AudioManager>().Play("Theme");
        FindObjectOfType<AudioManager>().Stop("HipHop");
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("VolumeA", volume);
        
        
    }

    public void Update()
    {
 
       
        if (_gameOverUI != null)
        {
            if (Movement.Death == true)
            {

                _gameOverUI.SetActive(true);
                Movement movement = FindObjectOfType<Movement>();
                if(movement != null)
                    movement._coolDownImageLarge.gameObject.SetActive(false);
                if(Score._reward > 0)
                {
                    _gameOverRewardUI.SetActive(true);
                    rewardNumber = gameObject.transform.GetChild(0).GetChild(6).GetChild(3).GetComponent<Text>();
                    rewardNumber.text = "+ " + Score._reward;
                    
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _gameOverRewardUI.SetActive(false);
                    GoldCurrency GC = FindObjectOfType<GoldCurrency>();
                    GC.AddMoneyToBank(Score._reward);
                    Score._reward = 0;
                }
            }
            else
                _gameOverUI.SetActive(false);
        }
    }
}
