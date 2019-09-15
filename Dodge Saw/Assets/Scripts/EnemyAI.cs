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
    public Color[] colors;
    public GameObject Particle1;
    public GameObject Particle2;
    #endregion
    Camera camera;
    float time;
    int index = 0;
    bool shouldChange = false;
    bool hasChanged = false;

    private void Awake()
    {
        camera = Camera.main;
        
    }
    

    private void Start()
    {
        time = _startTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        //KeepInBounds();
        Timer();
        if(shouldChange)
        {

            var startColor = camera.backgroundColor;
            var endColor = colors[0];
            if (index + 1 < colors.Length)
            {
                endColor = colors[index + 1];
            }
        

            Color newColor = Color.Lerp(startColor, endColor, Time.deltaTime * 5);
            SetColor(newColor);
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

    public void Timer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            SpawnEnemies();
            time = Random.Range(5,_startTime);
        }
    }

    public void SetColor(Color color)
    {
        
        camera.backgroundColor = color;

       

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
            index = 0;
            shouldChange = true;
            Particle1.SetActive(false);
            Particle2.SetActive(true);
            GameObject EnemyClone = Instantiate(enemies[0], _position, Quaternion.identity);
            Destroy(EnemyClone, 10f);
        }
        else if (Score._score >= 100 && Score._score < 199)
        {
            index = 1;
            shouldChange = true;
            Particle1.SetActive(true);
            Particle2.SetActive(false);
            GameObject EnemyClone = Instantiate(enemies[2], _position, Quaternion.identity);
            Destroy(EnemyClone, 5f);
        }
        else if (Score._score >= 200 && Score._score < 399)
        {
            index = 0;
            shouldChange = true;
            
            Particle1.SetActive(false);
            Particle2.SetActive(true);
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0,3)], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
        }
        else if(Score._score>= 400)
        {
            index = 1;
                shouldChange = true;
            
            Particle1.SetActive(true);
            Particle2.SetActive(false);
            GameObject EnemyClone = Instantiate(enemies[Random.Range(0, 3)], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
            GameObject EnemyClone1 = Instantiate(enemies[Random.Range(0, 3)], _position, Quaternion.identity);
            Destroy(EnemyClone1, 15f);
        }
        else
        {
           
            //SetColor(colors[index]);
            Particle1.SetActive(true);
            Particle2.SetActive(false);
            GameObject EnemyClone = Instantiate(enemies[1], _position, Quaternion.identity);
            Destroy(EnemyClone, 15f);
        }
        
    }
  
}
