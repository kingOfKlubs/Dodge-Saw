using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public GameObject _Player;
    public GameObject _deathEffect;
    Rigidbody _rigid;
    public Vector2 _moveDirection;
    public Movement movement;
    public ParticleSystem _ring;
    public LayerMask layer;

    Touch touch;
    float _moveSpeed;
    Transform playerPos;
    public float _dst;
    Vector2 _velocity;
    Animator anim;
    Vector2 _direction;
    Ray _ray;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _Player = GameObject.FindGameObjectWithTag("Player");
        movement = _Player.GetComponent<Movement>();
        _moveSpeed = movement._moveSpeed;
        playerPos = _Player.transform;
        Ray _ray = new Ray(transform.position, _Player.transform.position);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, layer))
        {
            _moveDirection = (playerPos.position - transform.position).normalized;

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

        Reflect();
        KeepInBounds();
        _rigid.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;

    }

    public void KeepInBounds()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f))
        {
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0f, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
        }
    }

    private void Reflect()
    {
        _ray = new Ray(transform.position, _moveDirection);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            _rigid.velocity = Vector2.Reflect(_moveDirection, _hit.normal);
            _moveDirection = _rigid.velocity;
        }

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
