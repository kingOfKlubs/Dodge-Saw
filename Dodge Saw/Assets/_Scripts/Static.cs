﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    public GameObject _Player;
    public GameObject _deathEffect;
    Rigidbody _rigid;
    public Vector2 _moveDirection;
    public Movement movement;
    public ParticleSystem _ring;

    Animator anim;
    Touch touch;
    float _moveSpeed;
    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _Player = GameObject.FindGameObjectWithTag("Player");
        if (_Player != null)
        {
            movement = _Player.GetComponent<Movement>();
            _moveSpeed = movement._moveSpeed;
            playerPos = _Player.transform;
            Ray _ray = new Ray(transform.position, _Player.transform.position);
            RaycastHit _hit;
            if (Physics.Raycast(_ray, out _hit))
            {
                _moveDirection = Vector2.zero;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _moveSpeed = .3f;
                anim.speed = .3f;
            }
        }
        else
        {
            _moveSpeed = 5;
            anim.speed = 1;
        }

        _rigid.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;
    }



    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Movement.Death = true;
            Destroy(collision.gameObject);    
                GameObject clone = Instantiate(_deathEffect, collision.transform.position, collision.transform.rotation);
				FindObjectOfType<AudioManager>().Play("PlayerCrash");
				Destroy(clone, 7);
        }
    }
}