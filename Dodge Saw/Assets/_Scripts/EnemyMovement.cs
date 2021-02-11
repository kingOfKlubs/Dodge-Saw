using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject _deathEffect;
    public Vector2 _moveDirection;
    public Movement movement;
    public ParticleSystem _ring;
    public LayerMask layer;
    public float _dst;

    protected Rigidbody _rigid;

    GameObject _player;
    Animator anim;
    Touch touch;
    Transform playerPos;
    Vector2 _velocity;
    Vector2 _direction;
    Ray _ray;
    float _moveSpeed;

    public virtual void Initiate()
    {
        anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_deathEffect != null)
        {
            if (_player != null)
            {
                movement = _player.GetComponent<Movement>();
                _moveSpeed = movement._moveSpeed;
                playerPos = _player.transform;

                Ray _ray = new Ray(transform.position, _player.transform.position);
                RaycastHit _hit;
                if (Physics.Raycast(_ray, out _hit))
                {
                    _moveDirection = (playerPos.position - transform.position).normalized;
                }
            }
        }
    }

    public virtual void Move()
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
        else if (Input.GetKey("space"))
        {
            _moveSpeed = .3f;
            anim.speed = .3f;
        }
        else
        {
            _moveSpeed = 5;
            anim.speed = 1;
        }
        _rigid.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;
    }

    public virtual void OnTriggerEnter(Collider collision)
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
