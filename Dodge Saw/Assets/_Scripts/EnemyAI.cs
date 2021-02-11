using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Public Variables
    public Vector2 _position = new Vector2(1, 1);
    public Transform _Player;
    public float _startTime;
    public ParticleSystem ring;
    public GameObject[] enemies = new GameObject[3];
    #endregion

    Vector2 _2ndPosition;
    Vector2 _3rdPosition;
    float time;
    
    private void Start()
    {
        time = _startTime;
        
    }

    // Update is called once per frame
    void Update()
    {
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
        _2ndPosition = new Vector2(Random.Range(-2f, 2f), Random.Range(-4.5f, 4.5f));
        _3rdPosition = new Vector2(Random.Range(-2f, 2f), Random.Range(-4.5f, 4.5f));
        StartCoroutine("WaitForRing");
    }

    IEnumerator WaitForRing()
    {
        ParticleSystem RingClone = Instantiate(ring, _position, Quaternion.identity);
        if(Score._score >= 500 && Score._score < 649)
        {
            
            ParticleSystem RingClone1 = Instantiate(ring, _2ndPosition, Quaternion.identity);
        }
        else if(Score._score >= 650)
        {
            ParticleSystem RingClone1 = Instantiate(ring, _2ndPosition, Quaternion.identity);
            ParticleSystem RingClone2 = Instantiate(ring, _3rdPosition, Quaternion.identity);
        }

        yield return new WaitForSeconds(2);

        if (Score._score >= 100 && Score._score < 199)
        {
            GameObject EnemyClone = Instantiate(enemies[0], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        else if (Score._score >= 200 && Score._score < 349)
        {
            GameObject EnemyClone = Instantiate(enemies[2], _position, Quaternion.identity);
            Destroy(EnemyClone, 5f);
        }
        else if (Score._score >= 350 && Score._score < 499)
        {
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0,3)], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
        }
        else if(Score._score>= 500 && Score._score < 649)
        { 
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0, 3)], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
            GameObject EnemyClone1 = Instantiate(enemies[Random.Range(0, 3)], _2ndPosition, Quaternion.identity);
            Destroy(EnemyClone1, 15f);
        }
        else if (Score._score >= 650)
        {
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0, 3)], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
            GameObject EnemyClone1 = Instantiate(enemies[Random.Range(0, 3)], _2ndPosition, Quaternion.identity);
            Destroy(EnemyClone1, 15f);
            GameObject EnemyClone2 = Instantiate(enemies[Random.Range(0, 3)], _3rdPosition, Quaternion.identity);
            Destroy(EnemyClone1, 15f);
        }
        else
        {
            GameObject EnemyClone = Instantiate(enemies[1], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
        }
        Destroy(RingClone, 3);
    }

    
        
}
