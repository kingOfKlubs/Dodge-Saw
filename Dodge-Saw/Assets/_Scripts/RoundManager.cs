using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
	public GameObject Particle;
    EnemyAI enemyAI;
    public RoundStates _currentRoundState;
	int _round = 0;
	int _previousRound = 0;
    Camera camera;
    int index = 0;
    bool shouldChange = false;
    public Color[] colors;
    public GameObject Particle1;
    public GameObject Particle2;
    public GameObject Particle3;
    Color newColor;
    Color startColor;
    Color endColor;
    private void Awake()
    {
        camera = Camera.main;

    }

    private void Start()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
    }
    // Update is called once per frame
    void Update()
	{

		switch(_currentRoundState)
		{
			case RoundStates.Round:
                Round();
			break;
			case RoundStates.RoundTransition:
                RoundTransition();
		    break;
		}
	UpdateRound();

        if (shouldChange)
        {

            startColor = camera.backgroundColor;
            endColor = colors[0];
            if (index + 1 < colors.Length)
            {
                endColor = colors[index + 1];
            }

            newColor = Color.Lerp(startColor, endColor, Time.deltaTime * 5);
            StartCoroutine(LerpColor());
            
            if (newColor == endColor)
            {
                shouldChange = false;
                if (index + 1 < colors.Length)
                {
                    index++;
                }
                else
                    index = 0;
            }
        }
    }

	public RoundStates UpdateStates(RoundStates newRoundState)
	{
		_currentRoundState = newRoundState;
		return _currentRoundState;
	}

    void Round()
    {
        if (enemyAI != null)
        {
            if (enemyAI.gameObject.activeSelf == false)
                enemyAI.gameObject.SetActive(true);
        }
    }

    void RoundTransition()
	{
       
        enemyAI.gameObject.SetActive(false);
		Particle.SetActive(true);
		StartCoroutine(NewRound());  
		
	}

	void UpdateRound()
	{
		if (Score._score >= 100 && Score._score < 199)
		{
   			_round = 2;
            index = 0;
            
            Particle1.SetActive(false);
            Particle2.SetActive(true);
            InitializePlayerCharacteristics.SetWarpColor();

        }
		else if (Score._score >= 200 && Score._score < 349)
		{

			_round = 3;
            index = 1;
           
            Particle1.SetActive(true);
            Particle2.SetActive(false);
            InitializePlayerCharacteristics.SetWarpColor();

        }
		else if (Score._score >= 350 && Score._score < 499)
		{

			_round = 4;
            index = 0;
            
            Particle1.SetActive(false);
            Particle2.SetActive(true);
            InitializePlayerCharacteristics.SetWarpColor();
        }
		else if (Score._score >= 500 && Score._score < 649)
        { 
			_round = 5;
            index = 1;
           
            Particle1.SetActive(true);
            Particle2.SetActive(false);
            InitializePlayerCharacteristics.SetWarpColor();
        }
        else if (Score._score >= 650)
        {
            _round = 6;
            index = 0;

            Particle1.SetActive(false);
            Particle2.SetActive(true);
            InitializePlayerCharacteristics.SetWarpColor();
        }
        else
		{
			_round = 1;
			_previousRound = 1;
		}

        if (_round != _previousRound)
        {
            UpdateStates(RoundStates.RoundTransition);
            FindObjectOfType<AudioManager>().Play("EndOfRound");
        }
        
    }

	public enum RoundStates{Round, RoundTransition};

	IEnumerator NewRound()
	{
		_previousRound = _round;
        shouldChange = true;
        
        yield return new WaitForSeconds(2);
		Particle.SetActive(false);
        UpdateStates(RoundStates.Round);
        
	}
    IEnumerator LerpColor()
    {
        
        yield return new WaitForSeconds(2.0f);
       
        SetColor(newColor);
    }


    public void SetColor(Color color)
    {
        camera.backgroundColor = color;
    }


}
