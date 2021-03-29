﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    //References
    [Header("UI references")]
    [SerializeField] TMP_Text coinUIText;
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] Transform target;

    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();


    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float AnimDuration;
    //[SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;

    [SerializeField] LeanTweenType easeType;
    [SerializeField] float spread;

    Vector3 targetPosition;


    private int _c = 0;

    public int Coins
    {
        get { return _c; }
        set
        {
            _c = value;
            //update UI text whenever "Coins" variable is changed
            coinUIText.text = Coins.ToString();
        }
    }

    void Awake()
    {
        _c = PlayerPrefs.GetInt("Currency");
        coinUIText.text = PlayerPrefs.GetInt("Currency").ToString();
        //prepare pool
        PrepareCoins();
    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    void Animate(Vector3 collectedCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //check if there's coins in the pool
            if (coinsQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f);

                //animate coin to target position
                float duration = AnimDuration;
                targetPosition = target.position;
                coin.transform.LeanMove(targetPosition, duration)
                .setEase(easeType)
                .setOnComplete(() => {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                    Coins++;
                    target.LeanScale(new Vector3(1f, 1f, 1f), .05f);
                    target.LeanScale(new Vector3(.7111111f, .7111111f, .7111111f), .05f).setDelay(.05f);
                });
            }
            
        }
    }

    public void AddCoins(Vector3 collectedCoinPosition, int amount)
    {
        Animate(collectedCoinPosition, amount);
    }
}
       
