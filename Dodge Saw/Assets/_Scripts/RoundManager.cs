using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;
using UnityEngine.Playables;

public class RoundManager : MonoBehaviour {

    [System.Serializable]
    public class Round {
        public string name;
        public GameObject[] enemies;
        public int count;
        public float rate;
        public float spawnRate;

        public Round(string _name, GameObject[] _enemies, int _count, float _rate, float _spawnRate)
        {
            name = _name;
            enemies = _enemies;
            count = _count;
            rate = _rate;
            spawnRate = _spawnRate;
        }
    }

    #region Public Variables
    public List<Round> rounds;
    public GameObject[] enemyTypes;
    public Color[] colors;
    public GameObject Portal;
    public ParticleSystem ring;
    public VisualEffect warp;
    public VisualEffect altwarp;
    public float PortalAnimDuration = 5.15f;
    public float delay = 3;
    public float _speed = 13.6f;
    public float roundCountDown = 5;
    public float radius = .7f;
    public RoundStates _currentRoundState;
    public LayerMask spawnedObjectLayer;
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
    Vector2 _position;
    TextMeshProUGUI _roundCounter;
    float searchCountdown = 3;
    float transitionTime = 0;
    int _round = 0;
    int _nextRound = 0;
    int _previousRound = 0;
    int incrementalCount;
    int index = 1;
    int attempts = 0;
    int spawnAmountCounter = 1;
    int enemyCap = 3;
    bool shouldChange = false;
    bool incrementedEnemyCap = false;
    #endregion

    public enum RoundStates { RoundStart, RoundTransition, NewRound, RoundSpawn, RoundWaiting, RoundTutorial };

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
        coinSpawning = FindObjectOfType<CoinSpawning>();
        _roundCounter = FindObjectOfType<RoundCounter>().GetComponent<TextMeshProUGUI>();
        _roundCounter.text = (_nextRound + 1).ToString();
        if (PlayerPrefsX.GetBool("hasCompletedTutorial") == false) {
            UpdateStates(RoundStates.RoundTutorial);    
        }else
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
            case RoundStates.RoundTutorial:
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
                objInScene[i].transform.position = Vector3.Lerp(objInScene[i].transform.position, new Vector3(objInScene[i].transform.position.x, objInScene[i].transform.position.y, objInScene[i].transform.position.z - 10), .7f * Time.fixedDeltaTime);
                Destroy(objInScene[i], 3);
            }
        }
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
        UpdateStates(RoundStates.RoundSpawn);
    }

    // begins the round transition
    void RoundTransition()
    {
        coinSpawning.gameObject.SetActive(false);
        StartCoroutine(UpdateEffects(warp, altwarp)); //TODO make this swap 
        StartCoroutine(NewRound());
    }

    IEnumerator NewRound()
    {
        _previousRound = _round;
        shouldChange = true;
        PlayableDirector playable = _roundCounter.GetComponent<PlayableDirector>(); // note this playable should only be as long as the delay
        //PlayableDirector playerAction = FindObjectOfType<Movement>().GetComponent<PlayableDirector>(); // note this playbale should only be as long as the delay + PortalAnimDuration
        playable.Play();
        //playerAction.Play();
        yield return new WaitForSeconds(delay);
        _roundCounter.text = (_nextRound + 1).ToString();
        Portal.SetActive(true);

        yield return new WaitForSeconds(PortalAnimDuration);
        Portal.SetActive(false);
        playable.Stop();
        //playerAction.Stop();
        UpdateStates(RoundStates.RoundStart);
    }

    IEnumerator UpdateEffects(VisualEffect warp, VisualEffect altWarp)
    {
        if (_nextRound % 2 == 0)
        {
            altWarp.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime - .5f);
            warp.SetFloat("speed", _speed);
        }
        else if(_nextRound%2==1)
        {
            warp.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime - .5f);
            altWarp.SetFloat("speed", _speed);
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
            yield return new WaitForSeconds(round.rate); 
            // the number of counts should dictate how many times you runt spawn enemies
            for (int j = 0; j < round.count; j++)
            {
                if(j > 0)
                {
                    GameObject enemyTemp = round.enemies[Random.Range(0, round.enemies.Length)];
                    PreventOverlapingSpawn(enemyTemp, enemyTemp.GetComponent<EnemyMovement>()._lifeTime);
                }
                else
                    PreventOverlapingSpawn(round.enemies[i], round.enemies[i].GetComponent<EnemyMovement>()._lifeTime);
                yield return new WaitForSeconds(round.spawnRate);
            }
        }

        //change the state
        _currentRoundState = RoundStates.RoundWaiting;
        
        yield break;
    }

    IEnumerator SpawnEnemy(GameObject _enemy, Vector2 _position, float _lifetime)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);

        // This is where we will spawn enemies
        ParticleSystem RingClone = Instantiate(ring, _position, Quaternion.identity);
        Destroy(RingClone.gameObject, 3);
        yield return new WaitForSeconds(2);
        GameObject EnemyClone = Instantiate(_enemy, _position, Quaternion.identity);
    }

    void RoundCompleted()
    {
        Debug.Log("Round Completed!");

        //if we have reached the end of our presets start making rounds dynamically
        if (_nextRound + 1 > rounds.Count - 1)
        {
            Debug.Log("ALL PRESET ROUNDS COMPLETE! incrementing...");
            Debug.Log(_nextRound);
            GenerateRound();
        }
        else
        {
            _nextRound++;
        }

        Debug.Log("Next Round is: Round " + _nextRound);
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

    public void PreventOverlapingSpawn(GameObject _enemy, float _lifetime)
    {
        //check around the position we want to spawn at
        bool canSpawnHere = false;
        attempts = 0;

        while (!canSpawnHere) {
            _position = new Vector2(Random.Range(bottomRange.x + findingDimensions.padding, topRange.x - findingDimensions.padding), Random.Range(bottomRange.y + findingDimensions.padding, topRange.y - findingDimensions.padding));

            Collider[] collidersInsideOverlapSphere = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapSphereNonAlloc(_position, radius, collidersInsideOverlapSphere, spawnedObjectLayer);

            if (numberOfCollidersFound == 0) {
                break;  
            }
            else {
                Debug.Log("found " + collidersInsideOverlapSphere[0].name + " collider attempting again");
                Debug.Log("number of attempts are " + attempts);
                attempts++;
                if (attempts > 50) {
                    break;
                }
            }
        }
        StartCoroutine(SpawnEnemy(_enemy, _position, _lifetime));
    }

    public void GenerateRound()
    {
        // make a new round dynamically
        if (_nextRound % 2 == 0)
            incrementalCount = ++spawnAmountCounter;
        //this will cap our amount of spawns per wave to 5
        int cap = 5; // change this value to increase cap
        if (spawnAmountCounter > cap) 
        {
            incrementalCount = cap;
        }
        _nextRound++;


        int amountOfEnemies = Random.Range(_nextRound, _nextRound + 6); //random number of enemies base on round number
        GameObject[] newEnemyList = new GameObject[amountOfEnemies]; //array of enemies 
        for (int i = 0; i < newEnemyList.Length; i++)
        {
            if (incrementedEnemyCap)
            {
                newEnemyList[i] = enemyTypes[enemyCap];
                incrementedEnemyCap = false;
            }
            newEnemyList[i] = enemyTypes[Random.Range(0, enemyCap)];
        }

        //were going to scale the enemies incoorperated in the round. this is to add in enemies while the rounds increase in difficulty randomly
        if (enemyCap >= enemyTypes.Length)
        {
            enemyCap = enemyTypes.Length;
        }
        else
        {
            enemyCap++;
            incrementedEnemyCap = true;
        }


        float randomRate = Random.Range(5, 8);// rate between wave spawns
        float randomSpawnRate = Random.Range(1, 3);// rate between spawns per wave

        rounds.Add(new Round("Round " + _nextRound, newEnemyList, incrementalCount, randomRate, randomSpawnRate));
    }
}
