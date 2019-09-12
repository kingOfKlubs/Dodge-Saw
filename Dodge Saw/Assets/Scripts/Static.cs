using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    public GameObject _Player;
    Rigidbody2D _rigid;
    public Vector2 _moveDirection;
    public Movement movement;
    public ParticleSystem _ring;

    Touch touch;
    float _moveSpeed;
    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {

        _rigid = GetComponent<Rigidbody2D>();
        _Player = GameObject.FindGameObjectWithTag("Player");
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

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _moveSpeed = .3f;

            }
        }
        else
        {
            _moveSpeed = 5;

        }

        _rigid.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;


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

                Destroy(clone, 7);

            }
        }
    }
}
