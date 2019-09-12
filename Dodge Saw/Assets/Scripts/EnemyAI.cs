using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Public Variables
    public Vector2 _position = new Vector2(1, 1);
    public GameObject _enemy;
    public Transform _Player;
    public float _startTime;
    public ParticleSystem ring;
    public GameObject[] enemies = new GameObject[3]; 
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
        if (time <= 0)
        {
            SpawnEnemies();
            time = Random.Range(5,_startTime);
        }
    }

    //Spawn Coins randomly on the screen
    public void SpawnEnemies()
    {

        _position = new Vector2(Random.Range(-2f, 2f), Random.Range(-4.5f, 4.5f));
        
        StartCoroutine("WaitForRing");
        

    }

    IEnumerator WaitForRing()
    {
        ParticleSystem RingClone = Instantiate(ring, _position, Quaternion.identity);
        Destroy(RingClone, 3);
        yield return new WaitForSeconds(2);
        if (Score._score >= 50 && Score._score < 99)
        {
            GameObject EnemyClone = Instantiate(enemies[1], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        else if (Score._score >= 100 && Score._score < 199)
        {
            GameObject EnemyClone = Instantiate(enemies[2], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        else if (Score._score >= 200)
        {
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0,2)], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        else
        {
            GameObject EnemyClone = Instantiate(enemies[0], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        
    }
  
}
