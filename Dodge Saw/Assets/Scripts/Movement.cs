using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    #region Public Variables

    //public variables
    public float _dst;
    public float _moveSpeed;
    #endregion

    #region Private Variables
    Vector2 _velocity;
    Rigidbody2D _rigid;
    Vector2 _direction;
    Ray _ray;
    float _initialSpeed;

    #endregion

    Touch touch;


    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _velocity = new Vector2(1,1);
        _direction = this.transform.up;
        _initialSpeed = _moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _rigid.velocity = _velocity * _moveSpeed;
        KeepInBounds();
        Reflect();
        Move();

      float Xspeed = Input.GetAxis("Horizontal");
      float Yspeed = Input.GetAxis("Vertical");

        //_rigid.velocity = new Vector2(Xspeed, Yspeed);      
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
        _ray = new Ray(transform.position, _direction);
        RaycastHit _hit;
            if(Physics.Raycast(_ray, out _hit, _dst))
            {
                _velocity = Vector2.Reflect(_direction, _hit.normal);
                _direction = _velocity;
            }
        
    }

    //draws the raycast of the direction of velocity
    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(_ray);
    }

    //moves the pentagon based on touch input
    public void Move()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _moveSpeed = 1;
            }
        }
        else
            _moveSpeed = _initialSpeed;
    }


}
