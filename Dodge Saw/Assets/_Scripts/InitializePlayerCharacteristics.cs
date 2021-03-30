using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class InitializePlayerCharacteristics : MonoBehaviour
{
    public GameObject _playerPrefab;
    public Color defaultColor;
    public Gradient defaultGradient;
    public GameObject[] _playerSkinPrefab;
    public GameObject[] CosmeticItems;
    public GameObject[] _players;
    public TrailRenderer _currentTrail { get; set; }
    
    public static Color _playerColor;

    Color _deathEffectColor; 
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    RoundManager roundManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefsX.GetBool("HasPlayed"))
        {
            // Setting up the Player's Color
            _playerColor = defaultColor;
            PlayerPrefs.SetFloat("_playerColor.r", _playerColor.r);
            PlayerPrefs.SetFloat("_playerColor.g", _playerColor.g);
            PlayerPrefs.SetFloat("_playerColor.b", _playerColor.b);

            // Setting up the Player's Trail
            PlayerPrefsX.SetColor("_trailGradient1", defaultGradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_trailGradient2", defaultGradient.colorKeys[1].color);
            PlayerPrefsX.SetColor("_trailGradient3", defaultGradient.colorKeys[2].color);

            // Setting up the Player's Death Effect
            PlayerPrefsX.SetColor("_deathGradient1", defaultGradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_deathGradient2", defaultGradient.colorKeys[0].color);
            PlayerPrefsX.SetColor("_deathGradient3", defaultGradient.colorKeys[0].color);
            PlayerPrefsX.SetBool("HasPlayed", true);
        }
        
        SetPlayerColor();
        _playerPrefab.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
        SetTrailColor();
        _playerPrefab.transform.GetChild(0).GetComponent<TrailRenderer>().colorGradient = gradient;
        if(_players != null)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                if(_players[i].GetComponent<MeshRenderer>().sharedMaterials.Length > 2)
                {
                    Material[] meshes = _players[i].GetComponent<MeshRenderer>().sharedMaterials;
                    for (int j = 0; j < meshes.Length ; j++)
                    {
                        meshes[j].SetColor("_EmissionColor", _playerColor);
                    }
                    _players[i].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                }
                _players[i].GetComponentInChildren<TrailRenderer>().colorGradient = gradient;
            }
        }
        SetDeathColor();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SetSkin();
            SetCosmetic();
        }
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            SetStuntPlayer();
        }
    }

    public void SetSkin()
    {
        foreach (GameObject go in _players)
        {
            go.SetActive(false);
        }
        switch (PlayerPrefs.GetString("ChosenSkin"))
        {
            case ("Player Skin 1"):
                _players[1].SetActive(true);
                break;
            case ("Player Skin 2"):
                _players[2].SetActive(true);
                break;
            case ("Equinox"):
                _players[3].SetActive(true);
                break;
            default:
                _players[0].SetActive(true);
                break;
        }
    }

    public void SetCosmetic()
    {
        foreach (GameObject go in CosmeticItems)
        {
            go.SetActive(false);
        }
        switch (PlayerPrefs.GetString("ChosenCosmetic"))
        {
            case ("Wings"):
                CosmeticItems[0].SetActive(true);
                break;
            case ("Halo"):
                CosmeticItems[1].SetActive(true);
                break;
            case ("Crown"):
                CosmeticItems[2].SetActive(true);
                break;
            case ("Rockets"):
                CosmeticItems[3].SetActive(true);
                break;
            default:
                foreach (GameObject go in CosmeticItems)
                {
                    go.SetActive(false);
                }
                break;
        }
    }

    public void SetStuntPlayer() {
        foreach(GameObject go in _playerSkinPrefab) {
            go.SetActive(false);
        }

        switch (PlayerPrefs.GetString("ChosenSkin")) {
            case ("Player Skin 1"):
                _playerSkinPrefab[1].SetActive(true);
                _playerSkinPrefab[1].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                break;
            case ("Player Skin 2"):
                _playerSkinPrefab[2].SetActive(true);
                _playerSkinPrefab[2].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                break;
            case ("Equinox"):
                _playerSkinPrefab[3].SetActive(true);
                _playerSkinPrefab[3].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                _playerSkinPrefab[3].GetComponent<MeshRenderer>().sharedMaterials[2].SetColor("_EmissionColor", _playerColor);
                _playerSkinPrefab[3].GetComponent<MeshRenderer>().sharedMaterials[3].SetColor("_EmissionColor", _playerColor);
                break;
            default:
                _playerSkinPrefab[0].SetActive(true);
                _playerSkinPrefab[0].GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
                break;
        }
    }

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

        //set up the gradient with values from above
        gradient.SetKeys(colorKey, alphaKey);

        ParticleSystem[] deathGradient = _playerPrefab.GetComponent<Movement>()._deathEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < deathGradient.Length; i++)
        {
            ParticleSystem.ColorOverLifetimeModule deathColor;
            deathColor = deathGradient[i].colorOverLifetime;
            deathColor.color = gradient;
        }
    }
}
