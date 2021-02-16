using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoundManager : MonoBehaviour {

    public GameObject Particle;
    public RoundStates _currentRoundState;
    public Color[] colors;
    public VisualEffect warp;
    public VisualEffect altwarp;
    public float PortalAnimDuration = 5.15f;
    public float delay = 3;
    public float _speed = 13.6f;

    EnemyAI enemyAI;
    Camera camera;
    Color newColor;
    Color startColor;
    Color endColor;
    float transitionTime = 0;
    int _round = 0;
    int _previousRound = 0;
    int index = 0;
    bool shouldChange = false;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Start()
    {
        transitionTime = delay + PortalAnimDuration;
        enemyAI = FindObjectOfType<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_round != _previousRound)
        {
            UpdateStates(RoundStates.RoundTransition);
            FindObjectOfType<AudioManager>().Play("EndOfRound");
        }


        if (Score._score >= 100 && Score._score < 199)
        {
            _round = 2;
            index = 0;
            if(_currentRoundState != RoundStates.Round)
            {
                StartCoroutine(UpdateEffects(warp, altwarp));
            }
        }
        else if (Score._score >= 200 && Score._score < 349)
        {

            _round = 3;
            index = 1;
            if (_currentRoundState != RoundStates.Round)
            {
                StartCoroutine(UpdateEffects(altwarp, warp));
            }
        }
        else if (Score._score >= 350 && Score._score < 499)
        {

            _round = 4;
            index = 0;
            if (_currentRoundState != RoundStates.Round)
            {
                StartCoroutine(UpdateEffects(warp, altwarp));
            }
        }
        else if (Score._score >= 500 && Score._score < 649)
        {
            _round = 5;
            index = 1;
            if (_currentRoundState != RoundStates.Round)
            {
                StartCoroutine(UpdateEffects(altwarp, warp));
            }
        }
        else if (Score._score >= 650)
        {
            _round = 6;
            index = 0;
            if (_currentRoundState != RoundStates.Round)
            {
                StartCoroutine(UpdateEffects(warp, altwarp));
            }
        }
        else
        {
            _round = 1;
            _previousRound = 1;
        }

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

    public void UpdateStates(RoundStates newRoundState)
    {
        _currentRoundState = newRoundState;
        switch (_currentRoundState)
        {
            case RoundStates.Round:
                Round();
                break;
            case RoundStates.RoundTransition:
                RoundTransition();
                break;
        }
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
        StartCoroutine(NewRound());
    }

    IEnumerator UpdateEffects(VisualEffect turnSpeedZero, VisualEffect turnSpeedUp)
    {
        turnSpeedZero.SetFloat("speed", 0);
        yield return new WaitForSeconds(transitionTime);
        turnSpeedUp.SetFloat("speed", _speed);
    }

    public enum RoundStates { Round, RoundTransition };

    IEnumerator NewRound()
    {
        _previousRound = _round;
        shouldChange = true;

        yield return new WaitForSeconds(delay);
        Particle.SetActive(true);

        yield return new WaitForSeconds(PortalAnimDuration);
        Particle.SetActive(false);
        UpdateStates(RoundStates.Round);
    }
    IEnumerator LerpColor()
    {
        yield return new WaitForSeconds(transitionTime);
    
        SetColor(newColor);
    }


    public void SetColor(Color color)
    {
        camera.backgroundColor = color;
    }


}
