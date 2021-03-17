using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//credits to original script go to blackthornprod
public class TutorialManager : MonoBehaviour
{
    #region Public Variables
    public float _timeToWait = 0;
    public GameObject[] _popups;
    public bool _hasCollectedCoin;
    public ParticleSystem ring;
    public GameObject enemy;
    public GameObject panel;
    public GameObject HoldAnim;
    public GameObject SwipeAnim;
    #endregion

    #region Private Variables
    private RoundManager _roundManager;
    [SerializeField]
    private CoinSpawning _coinSpawning;
    private Movement _movement;
    [SerializeField]
    private int _popUpIndex;
    private Touch _touch;
    private bool _hasTouched;
    bool oneTimeCall = false;
    private Vector2 _position = new Vector2(1, 1);
    private bool _runTutorial;
    private IEnumerator coroutine;
    Vector2 topRange;
    Vector2 bottomRange;
    private FindingDimensions findingDimensions = new FindingDimensions();
    AudioManager audioManager;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (PlayerPrefsX.GetBool("hasCompletedTutorial") == false)
        {
            
            _roundManager = FindObjectOfType<RoundManager>(); //this needs to be reactivated after tutorial is over
            _movement = FindObjectOfType<Movement>();
            _coinSpawning = FindObjectOfType<CoinSpawning>();
            _coinSpawning._canSpawn = false;  //this needs to be reactivated after tutorial is over
            _runTutorial = true;
            HoldAnim.SetActive(true);
        }
        else
        {
            _runTutorial = false;
            panel.SetActive(false);
            //Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_runTutorial)
        {
            panel.SetActive(true);
            Tutorial();
        }
    }

    public void Tutorial()
    {
        for (int i = 0; i < _popups.Length; i++)
        {
            if (i == _popUpIndex)
            {
                _popups[i].SetActive(true);
            }
            else
                _popups[i].SetActive(false);
        }

        //TODO finish working on tutorial
        //were gonna instruct the player to play the game one step at a time
        // 1. teach the player to slow time by pressing the screen
        // 2. teach the player to move the player by swiping
        // 3. teach the player to collect coins
        // 4. teach the player to avoid enemyies

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            TouchPhase phase = _touch.phase;
        }

            
        if (_popUpIndex == 0 && _touch.phase == TouchPhase.Ended && _movement._distance <= 150 || _popUpIndex == 0 && Input.GetKeyUp("space")) {
            _hasTouched = true;
            HoldAnim.SetActive(false);
            SwipeAnim.SetActive(true);
            _popUpIndex++;
        }
        else if (_popUpIndex == 1 && _movement._distance >= 150 || _popUpIndex == 1 && Input.GetKeyDown(KeyCode.E))
        {
            SwipeAnim.SetActive(false);
            _popUpIndex++;
        }
        else if (_popUpIndex == 2 && !_hasCollectedCoin)
        {
            if (!oneTimeCall)
            {
                _coinSpawning.TutorialSpawnCoin(new Vector2(-1.68f, 0), _coinSpawning._coin);
                oneTimeCall = true;
            }
        }
        else if (_popUpIndex == 2 && _hasCollectedCoin)
        {
            _popUpIndex++;
            oneTimeCall = false;
        }
        else if (_popUpIndex == 3)
        {
            if (!oneTimeCall)
            {
                Invoke("SpawnEnemies", .001f);
                coroutine = WaitForTime(5);
                StartCoroutine(coroutine);
                oneTimeCall = true;
            }
        }
        else if(_popUpIndex == 4)
        {
            coroutine = WaitForTime(5);
            StartCoroutine(coroutine);
            oneTimeCall = false;
        }
        else if(_popUpIndex == 5)
        {
            if (!oneTimeCall) {
                PlayerPrefsX.SetBool("hasCompletedTutorial", true);
                _roundManager.UpdateStates(RoundManager.RoundStates.RoundStart);
                panel.SetActive(false);
                _runTutorial = false;
                oneTimeCall = true;
            }
        }
    }


    public void SpawnEnemies()
    {
        _position = new Vector2(1.85f, -2.61f);
        StartCoroutine("WaitForRing");
    }

    public void ResetTutorial()
    {
        PlayerPrefsX.SetBool("hasCompletedTutorial", false);
        Debug.Log(PlayerPrefsX.GetBool("hasCompletedTutorial"));
        audioManager.Play("ButtonPressed");
    }

    public void CompleteTutorial()
    {
        PlayerPrefsX.SetBool("hasCompletedTutorial", true);
        Debug.Log(PlayerPrefsX.GetBool("hasCompletedTutorial"));
    }

    IEnumerator WaitForRing()
    {
        ParticleSystem RingClone = Instantiate(ring, _position, Quaternion.identity);
    
        yield return new WaitForSeconds(2);

        GameObject EnemyClone = Instantiate(enemy, _position, Quaternion.identity);
        Destroy(EnemyClone, 5f);
        Destroy(RingClone, 3);
        
    }
    IEnumerator WaitForTime(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _popUpIndex++;
        
    }
}

