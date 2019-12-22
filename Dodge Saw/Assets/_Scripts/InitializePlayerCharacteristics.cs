using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayerCharacteristics : MonoBehaviour
{
    GameObject _playerPrefab;

    public static Color _playerColor;

    public TrailRenderer _currentTrail { get; set; }
    Color _trailColor;

    Color _enemyColor;

    Color _warpColor;

    Color _deathEffectColor;

    // Start is called before the first frame update
    void Start()
    {
        _playerPrefab = GameObject.FindGameObjectWithTag("Player");
        SetPlayerColor();
        _playerPrefab.GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", _playerColor);
        SetTrailColor();
        _playerPrefab.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = _trailColor;
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
        _trailColor.r = PlayerPrefs.GetFloat("_trailColor.r");
        _trailColor.g = PlayerPrefs.GetFloat("_trailColor.g");
        _trailColor.b = PlayerPrefs.GetFloat("_trailColor.b");
    }

}
