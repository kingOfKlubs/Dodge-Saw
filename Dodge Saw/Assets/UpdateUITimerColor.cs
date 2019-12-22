using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUITimerColor : MonoBehaviour
{

    public GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Color _color = GetComponent<Image>().color;
        _color = _player.GetComponent<MeshRenderer>().sharedMaterials[1].GetColor("_EmissionColor");
        _color.a = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
