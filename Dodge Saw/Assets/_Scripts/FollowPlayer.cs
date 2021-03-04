using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{

    Transform PlayerPos;
    GameObject[] TimerUI;

    private void Awake()
    {
        SetUIColor();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPos != null)
        {
            this.transform.position = PlayerPos.position;

        }
        this.transform.LookAt(Camera.main.transform);
    }

    public void SetUIColor()
    {
        TimerUI = GameObject.FindGameObjectsWithTag("TimerUI");
        
        foreach (GameObject timerUI in TimerUI)
        {
            Debug.Log(timerUI.name);
            GameObject _playerr = GameObject.FindGameObjectWithTag("Player");
            Color _playerColor = _playerr.GetComponent<MeshRenderer>().sharedMaterials[1].GetColor("_EmissionColor");
            _playerColor.a = .5f;
            timerUI.GetComponent<Image>().color = _playerColor;

        }
    }
}
