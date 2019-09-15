using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public GameObject _Player;
    Rigidbody2D _rigid;
    public Vector2 _moveDirection;
    public Movement movement;
    public ParticleSystem _ring;

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
        _rigid = GetComponent<Rigidbody2D>();
        _Player = GameObject.FindGameObjectWithTag("Player");
        movement = _Player.GetComponent<Movement>();
        _moveSpeed = movement._moveSpeed;
        playerPos = _Player.transform;
        Ray _ray = new Ray(transform.position, _Player.transform.position);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
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

        _rigid.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;
        Reflect();
        KeepInBounds();

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
        if (Physics.Raycast(_ray, out _hit, _dst))
        {
            _rigid.velocity = Vector2.Reflect(_moveDirection, _hit.normal);
            _moveDirection = _rigid.velocity;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Movement.Death = true;
            Destroy(collision.gameObject);
            for (int i = 0; i < 6; i++)
            {
                GameObject clone = Instantiate(_Player, collision.transform.position, Quaternion.identity);
                clone.transform.localScale = new Vector3(10f, 10f, 10f);
                clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f)) * movement._moveSpeed * .5f;
				FindObjectOfType<AudioManager>().Play("PlayerCrash");
				Destroy(clone, 7);

            }
        }
    }
}
