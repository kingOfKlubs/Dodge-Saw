using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawning : MonoBehaviour
{

    #region Public Variables
    public Vector2 _position = new Vector2(1,1);
    public GameObject _coin;
    public float _startTime;
    #endregion

    float time;

    private void Start()
    {
        time = _startTime;
    }

    // Update is called once per frame
    void Update()
    {
        //KeepInBounds();
        Timer();
    }

    public void Timer()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            SpawnCoins();
            time = _startTime;
        }
    }

    //Spawn Coins randomly on the screen
    public void SpawnCoins()
    {
        
            _position = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-3,6));
            GameObject CoinClone = Instantiate(_coin, _position, Quaternion.identity);
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
