﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializePlayerCharacteristics : MonoBehaviour
{
    public GameObject _playerPrefab;

    public static Color _playerColor;

    public TrailRenderer _currentTrail { get; set; }

    Color _enemyColor;

    static Color _warpColor;
    static Color _warpColor2;
    static ParticleSystem.MainModule _warpParticles1;
    static ParticleSystem.MainModule _warpParticles2;
    static ParticleSystem.MainModule _altWarpParticles1;
    static ParticleSystem.MainModule _altWarpParticles2;
    
    Color _deathEffectColor; 

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    EnemyAI enemyAi;

    GameObject[] TimerUI;

    // Start is called before the first frame update
    void Awake()
    {
        SetPlayerColor();
        _playerPrefab.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
        SetTrailColor();
        _playerPrefab.transform.GetChild(0).GetComponent<TrailRenderer>().colorGradient = gradient;
        SetWarpColor();
        SetEnemiesColor();
        enemyAi = FindObjectOfType<EnemyAI>();
        enemyAi.enemies[1].GetComponent<MeshRenderer>().sharedMaterials[0].SetColor("_EmissionColor", _enemyColor);
        SetDeathColor();
        SetUIColor();
    }

    //public Color SetPlayerColor { set { value = _playerColor; } get { return _playerColor; } }

    public void SetPlayerColor()
    {
        _playerColor.r = PlayerPrefs.GetFloat("_playerColor.r");
        _playerColor.g = PlayerPrefs.GetFloat("_playerColor.g");
        _playerColor.b = PlayerPrefs.GetFloat("_playerColor.b");
    }
    public void SetTrailColor()
    { 
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[3];
        colorKey[0].color = PlayerPrefsX.GetColor("_trailGradient1");
        colorKey[0].time = 0.0f;
        colorKey[1].color = PlayerPrefsX.GetColor("_trailGradient2"); 
        colorKey[1].time = .667f;
        colorKey[2].color = PlayerPrefsX.GetColor("_trailGradient3"); 
        colorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = .667f;
        alphaKey[2].alpha = 1.0f;
        alphaKey[2].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

       
    }
    public static void SetWarpColor()
    {
        if (GameObject.FindGameObjectWithTag("Warp") != null)
        {
            _warpParticles1 = GameObject.FindGameObjectWithTag("Warp").GetComponent<ParticleSystem>().main;
            _warpParticles1.startColor = new ParticleSystem.MinMaxGradient(PlayerPrefsX.GetColor("_warpColor1"), PlayerPrefsX.GetColor("_warpColor2"));

        }
        if (GameObject.FindGameObjectWithTag("Warp1") != null)
        {
            _warpParticles2 = GameObject.FindGameObjectWithTag("Warp1").GetComponent<ParticleSystem>().main;
            _warpParticles2.startColor = new ParticleSystem.MinMaxGradient(PlayerPrefsX.GetColor("_warpColor1"), PlayerPrefsX.GetColor("_warpColor2")); 


        }
        if (GameObject.FindGameObjectWithTag("AltWarp") != null)
        {
            _altWarpParticles1 = GameObject.FindGameObjectWithTag("AltWarp").GetComponent<ParticleSystem>().main;
            _altWarpParticles1.startColor = new ParticleSystem.MinMaxGradient(PlayerPrefsX.GetColor("_altWarpColor1"), PlayerPrefsX.GetColor("_altWarpColor2"));


        }
        if (GameObject.FindGameObjectWithTag("AltWarp1") != null)
        {
            _altWarpParticles2 = GameObject.FindGameObjectWithTag("AltWarp1").GetComponent<ParticleSystem>().main;
            _altWarpParticles2.startColor = new ParticleSystem.MinMaxGradient(PlayerPrefsX.GetColor("_altWarpColor1"), PlayerPrefsX.GetColor("_altWarpColor2"));


        }
    }

    public void SetEnemiesColor()
    {
        _enemyColor = PlayerPrefsX.GetColor("EnemyColor");
    }

    public void SetDeathColor()
    {
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[3];
        colorKey[0].color = PlayerPrefsX.GetColor("_deathGradient1");
        colorKey[0].time = 0.0f;
        colorKey[1].color = PlayerPrefsX.GetColor("_deathGradient2");
        colorKey[1].time = .141f;
        colorKey[2].color = PlayerPrefsX.GetColor("_deathGradient3");
        colorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 0.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = .141f;
        alphaKey[2].alpha = 0.0f;
        alphaKey[2].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        ParticleSystem[] deathGradient = enemyAi.enemies[0].GetComponent<Static>()._deathEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < deathGradient.Length; i++)
        {
            ParticleSystem.ColorOverLifetimeModule deathColor;
            deathColor = deathGradient[i].colorOverLifetime;
            deathColor.color = gradient;
        }
        ParticleSystem[] deathGradient1 = enemyAi.enemies[1].GetComponent<Attack>()._deathEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < deathGradient1.Length; i++)
        {
            ParticleSystem.ColorOverLifetimeModule deathColor;
            deathColor = deathGradient1[i].colorOverLifetime;
            deathColor.color = gradient;
        }
        ParticleSystem[] deathGradient2 = enemyAi.enemies[2].GetComponent<Bounce>()._deathEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < deathGradient2.Length; i++)
        {
            ParticleSystem.ColorOverLifetimeModule deathColor;
            deathColor = deathGradient2[i].colorOverLifetime;
            deathColor.color = gradient;
        }
    }

    public void SetUIColor()
    {
        TimerUI = GameObject.FindGameObjectsWithTag("TimerUI");
        foreach(GameObject timerUI in TimerUI)
        {
            _playerColor.a = .5f;
            timerUI.GetComponent<Image>().color = _playerColor;

        }
    }
}