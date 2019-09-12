using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{

    public static int _score;
	public Text _scoreText;
    public Text _highScore;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        _highScore.text = "Top: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        _scoreText = GetComponent<Text>();
        _score = 0;
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;
        if(_score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            _highScore.text = "Top: " + _score.ToString();
            UI.SetActive(true);
        }

    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        _highScore.text = "0";
    }
}
