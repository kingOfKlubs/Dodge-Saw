using System.Collections;
using System.Collections.Generic;
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
    public Slider slider;
    public GameObject _gameOverUI;
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
    public bool canFindData;

    protected void OnEnable()
    {
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i)) && canFindData)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objectsToPersist[i]);
                file.Close();
            }
            else
            {
                //Do Nothing
            }
        }
    }
    protected void OnDisable()
    {
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i));
            var json = JsonUtility.ToJson(objectsToPersist[i]);
            bf.Serialize(file, json);
            file.Close();
        }
    }
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
        for (int i = 0; i < objectsToPersist.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i));
            var json = JsonUtility.ToJson(objectsToPersist[i]);
            bf.Serialize(file, json);
            file.Close();
        }
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
        Movement player = FindObjectOfType<Movement>();
        if(player != null)
        {
            Destroy(player.gameObject);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuName);
		FindObjectOfType<AudioManager>().Play("Theme");
        FindObjectOfType<AudioManager>().Stop("HipHop");
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("VolumeA", volume);
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
                    if(rewardNumber != null)
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
