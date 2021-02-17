using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoundManager : MonoBehaviour {

    [System.Serializable]
    public class Round {
        public string name;
        public GameObject[] enemies;
        public int count;
        public float rate;
    }

    #region Public Variables
    public RoundStates _currentRoundState;
    public Round[] rounds;
    public Color[] colors;
    public GameObject Particle;
    public ParticleSystem ring;
    public VisualEffect warp;
    public VisualEffect altwarp;
    public float PortalAnimDuration = 5.15f;
    public float delay = 3;
    public float _speed = 13.6f;
    public float lifetime = 10;
    public float roundCountDown = 5;
    public FindingDimensions findingDimensions = new FindingDimensions();
    #endregion

    #region Private Variables
    EnemyAI enemyAI;
    CoinSpawning coinSpawning;
    Camera camera;
    Color newColor;
    Color startColor;
    Color endColor;
    Vector2 topRange;
    Vector2 bottomRange;
    float searchCountdown = 3;
    float transitionTime = 0;
    int _round = 0;
    int _nextRound = 0;
    int _previousRound = 0;
    int index = 1;
    bool shouldChange = false;
    #endregion

    public enum RoundStates { RoundStart, RoundTransition, NewRound, RoundSpawn, RoundWaiting };

    //TODO fill rounds dynamically after presets end and bring in preventspawnoverlap method

    // Awake is called before Start
    private void Awake()
    {
        camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        transitionTime = delay + PortalAnimDuration;
        topRange = findingDimensions.GetWorldPosition(0, new Vector2(Screen.width, Screen.height));
        bottomRange = findingDimensions.GetWorldPosition(0, new Vector2(0, 0));
        enemyAI = FindObjectOfType<EnemyAI>();
        coinSpawning = FindObjectOfType<CoinSpawning>();
        UpdateStates(RoundStates.RoundStart);
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
            case RoundStates.RoundStart:
                RoundStart();
                break;
            case RoundStates.RoundTransition:
                RoundTransition();
                break;
            case RoundStates.NewRound:
                StartCoroutine(NewRound());
                break;
            case RoundStates.RoundSpawn:
                StartCoroutine(RoundSpawn(rounds[_nextRound]));
                break;
            case RoundStates.RoundWaiting:
                break;
        }
    }

    // Changes round based on score
    void ChangeRound()
    {
        if(_currentRoundState == RoundStates.RoundWaiting)
        {
            if (!EnemyIsAlive())
            {
                RoundCompleted();
            }
            else
            {
                return;
            }
        }
        if (_currentRoundState == RoundStates.RoundTransition)
        {
            GameObject[] objInScene = GameObject.FindGameObjectsWithTag("GameObject");
            for (int i = 0; i < objInScene.Length; i++)
            {
                objInScene[i].transform.position = Vector3.Lerp(objInScene[i].transform.position, new Vector3(objInScene[i].transform.position.x, objInScene[i].transform.position.y, objInScene[i].transform.position.z - 10), .3f * Time.fixedDeltaTime);
                Destroy(objInScene[i], 3);
            }
        }
        //if(roundCountDown <= 0)
        //{
        //    if (_currentRoundState != RoundStates.RoundSpawn)
        //    {
        //        UpdateStates(RoundStates.RoundSpawn);
        //    }
        //}
        //else
        //{
        //    roundCountDown -= Time.deltaTime;
        //}
    }

    // reactivates EnemyAI 
    void RoundStart()
    {
        if (coinSpawning != null)
        {
            if ( coinSpawning.gameObject.activeSelf == false)
            {
                coinSpawning.gameObject.SetActive(true);
            };
        }
        //if (enemyAI != null && coinSpawning != null)
        //{
        //    if (enemyAI.gameObject.activeSelf == false || coinSpawning.gameObject.activeSelf == false)
        //    {
        //        enemyAI.gameObject.SetActive(true);
        //        coinSpawning.gameObject.SetActive(true);
        //    };
        //}
        UpdateStates(RoundStates.RoundSpawn);
    }

    // begins the round transition
    void RoundTransition()
    {
        //enemyAI.gameObject.SetActive(false);
        coinSpawning.gameObject.SetActive(false);
        StartCoroutine(UpdateEffects(warp, altwarp)); //TODO make this swap 
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
        UpdateStates(RoundStates.RoundStart);
    }

    IEnumerator UpdateEffects(VisualEffect turnSpeedZero, VisualEffect turnSpeedUp)
    {
        if (_nextRound % 2 == 0)
        {
            turnSpeedUp.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime);
            turnSpeedZero.SetFloat("speed", _speed);
        }
        else if(_nextRound%2==1)
        {
            turnSpeedZero.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime);
            turnSpeedUp.SetFloat("speed", _speed);
        }
    }

    public void ChangeBackground()
    {
        if (shouldChange)
        {
            startColor = camera.backgroundColor;
            endColor = colors[index];

            newColor = Color.Lerp(startColor, endColor, Time.deltaTime * 5);
            StartCoroutine(LerpColor());

            if (newColor == endColor)
            {
                shouldChange = false;
                if (index + 1 > colors.Length - 1)
                {
                    index = 0;
                }
                else
                    index++;
            }
            //startColor = camera.backgroundColor;
            //endColor = colors[0];
            //if (index + 1 < colors.Length)
            //{
            //    endColor = colors[index + 1];
            //}

            //newColor = Color.Lerp(startColor, endColor, Time.deltaTime * 5);
            //StartCoroutine(LerpColor());

            //if (newColor == endColor)
            //{
            //    shouldChange = false;
            //    if (index + 1 < colors.Length)
            //    {
            //        index++;
            //    }
            //    else
            //        index = 0;
            //}
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

    IEnumerator RoundSpawn(Round round)
    {
        Debug.Log("Spawning Round: " + round.name);

        for (int i = 0; i < round.enemies.Length; i++)
        {
            // the number of counts should dictate how many times you runt spawn enemies
            for (int j = 0; j < round.count; j++)
            {
                StartCoroutine(SpawnEnemy(round.enemies[i]));
            }
            yield return new WaitForSeconds(round.rate); 
        }

        //change the state
        _currentRoundState = RoundStates.RoundWaiting;
        
        yield break;
    }

    IEnumerator SpawnEnemy(GameObject _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);

        // This is where we will find position and spawn enemies
        Vector2 _position = new Vector2(Random.Range(bottomRange.x + findingDimensions.padding, topRange.x - findingDimensions.padding), Random.Range(bottomRange.y + findingDimensions.padding, topRange.y - findingDimensions.padding));
        ParticleSystem RingClone = Instantiate(ring, _position, Quaternion.identity);
        Destroy(RingClone.gameObject, 3);
        yield return new WaitForSeconds(2);
        GameObject EnemyClone = Instantiate(_enemy, _position, Quaternion.identity);
        Destroy(EnemyClone, lifetime);
    }

    void RoundCompleted()
    {
        Debug.Log("Round Completed!");

        roundCountDown = transitionTime;

        if (_nextRound + 1 > rounds.Length - 1)
        {
            _nextRound = 0;
            Debug.Log("ALL ROUNDS COMPLETE! Looping...");
        }
        else
        {
            _nextRound++;
        }
        UpdateStates(RoundStates.RoundTransition);
        FindObjectOfType<AudioManager>().Play("EndOfRound");
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 3;
            EnemyMovement[] Enemies = GameObject.FindObjectsOfType<EnemyMovement>();
            Debug.Log(Enemies.Length);
            if (Enemies.Length == 0)
            {
                
                return false;
            }
        }
        return true;
    }
}
