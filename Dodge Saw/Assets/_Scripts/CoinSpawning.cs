using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CoinSpawning : MonoBehaviour
{

    #region Public Variables
    public float _startTime;
    public float _startTimeBronze;
    public float _startTimeGold;
    public float radius; // this is usually kept at 0.7
    public float raycastDistance = 100f;
    public float overlapTestBoxSize = 1f;
    public float animDuration = 1f;
    public bool _canSpawn;
    public Vector2 _position = new Vector2(1,1);
    public VisualEffect entryAnim;
    public GameObject _coin;
    public GameObject _bronzeCoin;
    public GameObject _goldCoin;
    public LayerMask spawnedObjectLayer;
    public FindingDimensions findingDimensions = new FindingDimensions();
    #endregion

    float time;
    float bTime;
    float gTime;
    int attempts = 0;
    Vector2 topRange ;
    Vector2 bottomRange;
    
    private void Start()
    {
        topRange = findingDimensions.GetWorldPosition(0, new Vector2(Screen.width, Screen.height));
        bottomRange = findingDimensions.GetWorldPosition(0, new Vector2(0, 0));

        time = _startTime;
        bTime = _startTimeBronze;
        gTime = _startTimeGold;
        _canSpawn = PlayerPrefsX.GetBool("hasCompletedTutorial");
    }

    // Update is called once per frame
    void Update()
    {
        //KeepInBounds();
        if(_canSpawn)
            Timer();
    }

    public void Timer()
    {
        time -= Time.deltaTime;
        bTime -= Time.deltaTime;
        gTime -= Time.deltaTime;

        if (time <= 0)
        {
            PreventOverlapingSpawn(_coin);
            time = Random.Range(4,_startTime);
        }
        if (bTime <= 0)
        {
            PreventOverlapingSpawn(_bronzeCoin);
            bTime = Random.Range(2.7f, _startTimeBronze);
        }
        if (gTime <= 0)
        {
            PreventOverlapingSpawn(_goldCoin);
            gTime = Random.Range(10f, _startTimeGold);
        }
    }

    //Spawn Coins randomly on the screen
    IEnumerator SpawnCoins(Vector3 _position, GameObject coinType)
    {
        Vector4 coinColor = coinType.GetComponent<MeshRenderer>().sharedMaterial.GetVector("_EmissionColor");
        entryAnim.SetVector4("Flash Color", coinColor);
        VisualEffect leadIn = Instantiate(entryAnim, _position, Quaternion.identity);

        Destroy(leadIn.gameObject, 2);
        yield return new WaitForSeconds(animDuration);
        GameObject CoinClone = Instantiate(coinType, _position, Quaternion.identity);
        Destroy(CoinClone, 15f);
    }

    public void PreventOverlapingSpawn(GameObject coinType) {
        
        _position = new Vector2(Random.Range(bottomRange.x + findingDimensions.padding, topRange.x - findingDimensions.padding), Random.Range(bottomRange.y + findingDimensions.padding,topRange.y - findingDimensions.padding));    

        //check around the position we want to spawn at
        Collider[] collidersInsideOverlapSphere = new Collider[1];
        int numberOfCollidersFound = Physics.OverlapSphereNonAlloc(_position, radius, collidersInsideOverlapSphere, spawnedObjectLayer);


        if (numberOfCollidersFound == 0) {
            StartCoroutine(SpawnCoins(_position, coinType));
            attempts = 0;
        }
        else {
            Debug.Log("found " + collidersInsideOverlapSphere[0].name + " collider attempting again");
            attempts++;
            if(attempts < 50) {
                PreventOverlapingSpawn(coinType);
            }
            else {
                Debug.Log("maxed attempts reached, spawning anyway");
                StartCoroutine(SpawnCoins(_position, coinType));
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_position, radius);
    }
}


    //Keep the coins from spawning outside of the screen
    //public void KeepInBounds()
    //{
    //    if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f))
    //    {
    //        screenPosition.x = Mathf.Clamp(screenPosition.x, 0f, Screen.width);
    //        screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height);
    //        Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    //        this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);


    //    }
    //}

