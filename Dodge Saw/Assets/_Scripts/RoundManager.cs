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
        public float lifetime;

        public Round(string _name, GameObject[] _enemies, int _count, float _rate, float _lifetime)
        {
            name = _name;
            enemies = _enemies;
            count = _count;
            rate = _rate;
            lifetime = _lifetime;
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
    //Vector2 _position;
    float searchCountdown = 3;
    float transitionTime = 0;
    int _round = 0;
    int _nextRound = 0;
    int _previousRound = 0;
    int index = 1;
    int attempts = 0;
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
        //enemyAI = FindObjectOfType<EnemyAI>();
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
        Portal.SetActive(true);

        yield return new WaitForSeconds(PortalAnimDuration);
        Portal.SetActive(false);
        UpdateStates(RoundStates.RoundStart);
    }

    IEnumerator UpdateEffects(VisualEffect warp, VisualEffect altWarp)
    {
        if (_nextRound % 2 == 0)
        {
            altWarp.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime);
            warp.SetFloat("speed", _speed);
        }
        else if(_nextRound%2==1)
        {
            warp.SetFloat("speed", 0);
            yield return new WaitForSeconds(transitionTime);
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
                    PreventOverlapingSpawn(round.enemies[Random.Range(0, round.enemies.Length)], round.lifetime);
                }
                else
                    PreventOverlapingSpawn(round.enemies[i], round.lifetime);
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
        Destroy(EnemyClone, _lifetime);
    }

    void RoundCompleted()
    {
        Debug.Log("Round Completed!");
        Debug.Log("Next Round " + _nextRound);

        //if we have reached the end of our presets start making rounds dynamically
        if (_nextRound + 1 > rounds.Count - 1)
        {
            Debug.Log("ALL PRESET ROUNDS COMPLETE! incrementing...");
            // instead make a new round dynamically
            int randomCount = _nextRound; // this will probably need to be chaged if we have more than 3 presets
            _nextRound++;


            int amountOfEnemies = Random.Range(_nextRound, _nextRound + 6); //random number of enemies base on round number
            GameObject[] newEnemyList = new GameObject[amountOfEnemies]; //array of enemies 
            for (int i = 0; i < newEnemyList.Length; i++)
            {
                newEnemyList[i] = enemyTypes[Random.Range(0, enemyTypes.Length)];
            }
            
            float randomRate = Random.Range(5, 8);// rate between spawns
            float lifetime = Random.Range(5, 11);// the amount of time an enemy will be alive 

            rounds.Add(new Round("Round " + _nextRound, newEnemyList, randomCount, randomRate, lifetime));
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

    public void PreventOverlapingSpawn(GameObject _enemy, float _lifetime)
    {
        //check around the position we want to spawn at
        Vector2 _position = new Vector2(Random.Range(bottomRange.x + findingDimensions.padding, topRange.x - findingDimensions.padding), Random.Range(bottomRange.y + findingDimensions.padding, topRange.y - findingDimensions.padding));

        Collider[] collidersInsideOverlapSphere = new Collider[1];
        int numberOfCollidersFound = Physics.OverlapSphereNonAlloc(_position, radius, collidersInsideOverlapSphere, spawnedObjectLayer);


        if (numberOfCollidersFound == 0)
        {
            StartCoroutine(SpawnEnemy(_enemy, _position, _lifetime));
            attempts = 0;
        }
        else
        {
            Debug.Log("found " + collidersInsideOverlapSphere[0].name + " collider attempting again");
            attempts++;
            if (attempts < 10)
            {
                Debug.Log("maxed attempts reached, spawning anyway");
                PreventOverlapingSpawn(_enemy, _lifetime);
            }
            else
            {
                StartCoroutine(SpawnEnemy(_enemy, _position, _lifetime));
            }
        }
    }
}
