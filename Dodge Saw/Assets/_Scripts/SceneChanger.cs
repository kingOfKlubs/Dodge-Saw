﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public AudioMixer mixer;
    public Animator anim;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject store;
    public Slider slider;
    public GameObject _gameOverUI;
    public GameObject _gameOver;
    public GameObject _gameOverRewardUI;
    [SerializeField]
    public string startGameName;
    public string mainMenuName;
    public TextMeshProUGUI rewardNumber;
    AudioManager AM;
    
    [Header("Meta")]
    public string persisterName;
    [Header("Scriptable Objects")]
    public List<ScriptableObject> objectsToPersist;
   
    protected void OnEnable()
    {
        //for (int i = 0; i < objectsToPersist.Count; i++)
        //{
        //    if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i)))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i), FileMode.Open);
        //        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objectsToPersist[i]);
        //        file.Close();
        //    }
        //    else
        //    {
                
        //    }
        //}
    }

    protected void OnDisable()
    {
        //for (int i = 0; i < objectsToPersist.Count; i++)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i));
        //    var json = JsonUtility.ToJson(objectsToPersist[i]);
        //    bf.Serialize(file, json);
        //    file.Close();
        //}
    }

    public void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume");
        mixer.SetFloat("Volume", volume);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startGameName);
        AM = FindObjectOfType<AudioManager>();
        //slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
    }     

    public void QuitGame()
    {
        //for (int i = 0; i < objectsToPersist.Count; i++)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i));
        //    var json = JsonUtility.ToJson(objectsToPersist[i]);
        //    bf.Serialize(file, json);
        //    file.Close();
        //}
        Application.Quit();
    }

    public void Options()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
		FindObjectOfType<AudioManager>().Play("HipHop");
        if(anim != null)
        {
            anim.SetBool("Options", true);
        }
        if (mainMenu != null)
            mainMenu.SetActive(false);
        if (options != null)
            options.SetActive(true);
    }
    public void Store()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
        FindObjectOfType<AudioManager>().Play("HipHop");
        if (anim != null)
        {
            anim.SetBool("Store", true);
        }
        if (store != null)
            store.SetActive(true);
        if (mainMenu != null)
            mainMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Movement player = FindObjectOfType<Movement>();
        if(player != null)
        {
            Destroy(player.gameObject);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
		FindObjectOfType<AudioManager>().Play("Theme");
        FindObjectOfType<AudioManager>().Stop("HipHop");
    }

    public void Main()
    {
        if(anim != null)
        {
            anim.SetBool("Options", false);
            anim.SetBool("Store", false);
        }
        Time.timeScale = 1;
        if(options != null)
            options.SetActive(false);
        if(mainMenu != null)
            mainMenu.SetActive(true);
        if(store != null)
            store.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Theme");
        FindObjectOfType<AudioManager>().Stop("HipHop");
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    float timer = 2;

    public void Update()
    {
        if (_gameOverUI != null)
        {
            if (Movement.Death == true)
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    _gameOverUI.SetActive(true);
                    FindObjectOfType<Score>().ShowScore();
                    if(_gameOver != null)
                        _gameOver.gameObject.SetActive(true);
                    Movement movement = FindObjectOfType<Movement>();
                    if (movement != null)
                        movement._coolDownImageLarge.gameObject.SetActive(false);
                    if (Score._reward > 0)
                    {
                        _gameOverRewardUI.SetActive(true);
                        if (rewardNumber != null)
                            rewardNumber.text = "+ " + Score._reward;

                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        _gameOverRewardUI.SetActive(false);
                        GoldCurrency GC = FindObjectOfType<GoldCurrency>();
                        GC.AddMoneyToBank(Score._reward);
                        Score._reward = 0;
                    }
                    Time.timeScale = 0;
                }
            }
            else
                _gameOverUI.SetActive(false);
        }
    }
}
