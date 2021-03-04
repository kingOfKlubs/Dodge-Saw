using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyMovement : MonoBehaviour
{

    public GameObject _playerDeathEffect;
    public VisualEffect _destroyEffect;
    public Vector2 _moveDirection;
    public Movement movement;
    public LayerMask layer;
    public float _dst;
    public float _lifeTime;

    protected Rigidbody _rigid;
    protected bool dead = false;
    protected GameObject _player;
    protected Animator anim;
    protected Touch touch;
    protected Transform playerPos;
    Vector2 _velocity;
    Vector2 _direction;
    Ray _ray;
    protected float _moveSpeed;
    float time;

    public virtual void Initiate()
    {
        time = _lifeTime;
        anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_playerDeathEffect != null)
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
        time -= Time.deltaTime;
        if(time <= 0) {
            Death();
        }


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
            GameObject clone = Instantiate(collision.GetComponent<Movement>()._deathEffect, collision.transform.position, collision.transform.rotation);
            FindObjectOfType<AudioManager>().Play("PlayerCrash");
            Destroy(clone, 3);
        }
    }

    public virtual void Death() {
        VisualEffect deathClone = Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Destroy(deathClone.gameObject, 2);
        Destroy(this.gameObject);
    }
}
