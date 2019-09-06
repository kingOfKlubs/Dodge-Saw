using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{

    public static int _score;
	Text _scoreText;  

    // Start is called before the first frame update
    void Start()
    {
        _scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;

    }
}
