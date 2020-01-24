using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawning : MonoBehaviour
{

    #region Public Variables
    public Vector2 _position = new Vector2(1,1);
    public GameObject _coin;
    public GameObject _bronzeCoin;
    public GameObject _goldCoin;
    public float _startTime;
    public float _startTimeBronze;
    public float _startTimeGold;
    public bool _canSpawn;
    #endregion

    float time;
    float bTime;
    float gTime;

    private void Start()
    {
        time = _startTime;
        bTime = _startTimeBronze;
        gTime = _startTimeGold;
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
            SpawnCoins(_coin);
            time = Random.Range(4,_startTime);
        }
        if (bTime <= 0)
        {
            SpawnCoins(_bronzeCoin);
            bTime = Random.Range(2.7f, _startTimeBronze);
        }
        if (gTime <= 0)
        {
            SpawnCoins(_goldCoin);
            gTime = Random.Range(10f, _startTimeGold);
        }
    }

    //Spawn Coins randomly on the screen
    public void SpawnCoins(GameObject coinType)
    {
        
            _position = new Vector2(Random.Range(-2f, 2f), Random.Range(-4.5f,4.5f));
            GameObject CoinClone = Instantiate(coinType, _position, Quaternion.identity);
            Destroy(CoinClone, 15f);
            
        
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
}
