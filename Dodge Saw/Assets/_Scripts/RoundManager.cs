using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoundManager : MonoBehaviour {

    #region Public Variables
    public RoundStates _currentRoundState;
    public Color[] colors;
    public GameObject Particle;
    public VisualEffect warp;
    public VisualEffect altwarp;
    public AnimationClip animClip;
    public float PortalAnimDuration = 5.15f;
    public float delay = 3;
    public float _speed = 13.6f;
    #endregion

    #region Private Variables
    EnemyAI enemyAI;
    CoinSpawning coinSpawning;
    Camera camera;
    Color newColor;
    Color startColor;
    Color endColor;
    float transitionTime = 0;
    int _round = 0;
    int _previousRound = 0;
    int index = 0;
    bool shouldChange = false;
    #endregion

    public enum RoundStates { Round, RoundTransition };

    // Awake is called before Start
    private void Awake()
    {
        camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        transitionTime = delay + PortalAnimDuration;
        enemyAI = FindObjectOfType<EnemyAI>();
        coinSpawning = FindObjectOfType<CoinSpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeRound();
        ChangeBackground();
    }

    // Updates the state and calls appropriate method
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

    // Changes round based on score
    void ChangeRound()
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
            if (_currentRoundState != RoundStates.Round)
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
    }

    // reactivates EnemyAI 
    void Round()
    {
        if (enemyAI != null || coinSpawning != null)
        {
            if (enemyAI.gameObject.activeSelf == false || coinSpawning.gameObject.activeSelf == false)
            {
                enemyAI.gameObject.SetActive(true);
                coinSpawning.gameObject.SetActive(true);
            };
        }
    }

    // begins the round transition
    void RoundTransition()
    {
        enemyAI.gameObject.SetActive(false);
        coinSpawning.gameObject.SetActive(false);
        GameObject[] objInScene = GameObject.FindGameObjectsWithTag("Coin");
        for (int i = 0; i < objInScene.Length; i++)
        {
            objInScene[i].transform.position = Vector3.Lerp(objInScene[i].transform.position, new Vector3(objInScene[i].transform.position.x, objInScene[i].transform.position.y, objInScene[i].transform.position.z - 10), 1) * Time.deltaTime;
            Destroy(objInScene[i], 3);
        }
        StartCoroutine(NewRound());
    }

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

    IEnumerator UpdateEffects(VisualEffect turnSpeedZero, VisualEffect turnSpeedUp)
    {
        turnSpeedZero.SetFloat("speed", 0);
        yield return new WaitForSeconds(transitionTime);
        turnSpeedUp.SetFloat("speed", _speed);
    }

    public void ChangeBackground()
    {
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
