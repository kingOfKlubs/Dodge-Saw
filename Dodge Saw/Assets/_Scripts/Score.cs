using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Score : MonoBehaviour
{
    [SerializeField]
    public static int _score;
    public static int _scoreRecord;
    public static int _reward;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _highScore;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        if(_highScore != null)
        _highScore.text = "Top: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        _scoreText = GetComponent<TextMeshProUGUI>();
        _score = 0;
        _scoreRecord = 0;
        if(UI != null)
        UI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(_scoreText != null)
        _scoreText.text = "Score: " + _score;
        if (_score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            _highScore.text = "Top: " + _score.ToString();
            UI.SetActive(true);
        }
        GetMoney();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        _highScore.text = "0";
    }

    public void GetMoney()
    {
        if (_scoreRecord >= 100)
        {
            _scoreRecord = 0;
            _reward++;
        }
    }
}
