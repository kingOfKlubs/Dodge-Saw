using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Movement : MonoBehaviour
{
    #region Public Variables

    //public variables
    public float _dst;
    public float _moveSpeed;
    public static bool Death;
    public ParticleSystem Warp;
    public float _coolDownTime = -5;
    public Image _coolDownImage;
    public float _startTime = 0;
    #endregion

    #region Private Variables
    Vector2 _velocity;
    Rigidbody2D _rigid;
    Vector2 _direction;
    Ray _ray;
    float _initialSpeed;
    float angle;
    Animator anim;
    float _timeStopped;
    float nextTimeCanStop = 0;
    public bool _needCoolDown = false;
    Touch touch;
    private GameStates currentState;
    bool canSlowTime;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        Death = false;
        _rigid = GetComponent<Rigidbody2D>();
        _velocity = new Vector2(0, 1);
        _direction = this.transform.up;
        _initialSpeed = _moveSpeed;
        anim = GetComponent<Animator>();
        _timeStopped = _startTime;
    }

    // Update is called once per frame
    void Update()
    {
        _rigid.velocity = _velocity * _moveSpeed;
        KeepInBounds();
        Reflect();
        Move();
        
        SwitchStates();
        CoolDown();    
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
        if (Physics.Raycast(_ray, out _hit, _dst))
        {
            _velocity = Vector2.Reflect(_direction, _hit.normal);
            _direction = _velocity;
        }

    }

    public GameStates UpdateStates(GameStates newGameState)
       {
        currentState = newGameState;
        return currentState;
       }

    public void SwitchStates()
    {
        switch(currentState)
        {
            case GameStates.NormalTime:
                MoveNormal();
                break;
            case GameStates.SlowTime:
                MoveSlow();  
                break;
            case GameStates.CoolTime:
                MoveCool();
                break;
        }
    }

    public void MoveNormal()
    {
        _timeStopped = _startTime;
        _moveSpeed = _initialSpeed;
        anim.speed = 1;
        Warp.playbackSpeed = 1;
        if(_coolDownImage.fillAmount != 1)
            _coolDownImage.fillAmount = 1;


    }

    public void MoveCool()
    {
        _timeStopped = _startTime;
        _moveSpeed = _initialSpeed;
        anim.speed = 1;
        Warp.playbackSpeed = 1;
        float counter = 5;
        counter -= Time.deltaTime;
        _coolDownImage.fillAmount += _coolDownTime / counter * Time.deltaTime; 

    }



    public void MoveSlow()
    {
        _coolDownImage.fillAmount -= 1 / _startTime * Time.deltaTime;
        _moveSpeed = .3f;
        anim.speed = .3f;
        Warp.playbackSpeed = .3f;
    }
    //moves the pentagon based on touch input
    public void Move()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    UpdateStates(GameStates.SlowTime);

        //}
        //else
        //{
        //    UpdateStates(GameStates.NormalTime);
        //}


        if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                _timeStopped -= Time.deltaTime;
                

                if (_needCoolDown == false && _timeStopped > 0 && canSlowTime)
                    UpdateStates(GameStates.SlowTime);
                else if(_needCoolDown == false && _timeStopped <= 0)
                {
                    UpdateStates(GameStates.CoolTime);
                    _needCoolDown = true;
                }
                //if (touch.phase == TouchPhase.Began)
                //{
                //}
            }
        else if(canSlowTime == false)
        {
            UpdateStates(GameStates.CoolTime);
        }
        else
        {
            UpdateStates(GameStates.NormalTime);
        }
        

        Debug.Log("_timedStopped is " + _timeStopped);

        float xSpeed = CrossPlatformInputManager.GetAxis("Horizontal");
        float ySpeed = CrossPlatformInputManager.GetAxis("Vertical");
        if (Mathf.Abs(xSpeed) < .9f && Mathf.Abs(ySpeed) < .9f)
            return;

        angle = Mathf.Atan2(xSpeed, ySpeed);
        angle = Mathf.Rad2Deg * angle;
        angle += Camera.main.transform.eulerAngles.y;

        if (angle < 120 && angle > 60)
        {
            _velocity = new Vector2(1, 0);
            _direction = _velocity;
        }
        else if (angle < 60 && angle > 40)
        {
            _velocity = new Vector2(1, 1);
            _direction = _velocity;
        }
        else if (angle < 40 && angle > -40)
        {
            _velocity = new Vector2(0, 1);
            _direction = _velocity;
        }
        else if (angle < -40 && angle > -60)
        {
            _velocity = new Vector2(-1, 1);
            _direction = _velocity;
        }
        else if (angle < -60 && angle > -120)
        {
            _velocity = new Vector2(-1, 0);
            _direction = _velocity;
        }
        else if (angle < -120 && angle > -140)
        {
            _velocity = new Vector2(-1, -1);
            _direction = _velocity;
        }
        else if (angle < -140 || angle > 140) 
        {
            _velocity = new Vector2(0, -1);
            _direction = _velocity;
        }
        else if (angle < 140 && angle > 120)
        {
            _velocity = new Vector2(1, -1);
            _direction = _velocity;
        }
       //Debug.Log(angle);
    }

    void CoolDown()
    {
        Debug.Log("Time.time is " + Time.time);
        Debug.Log("Next time can stop is " + nextTimeCanStop);

        
            if (_needCoolDown && Time.time > nextTimeCanStop)
            {
                nextTimeCanStop = Time.time + _coolDownTime;
                _needCoolDown = false;
                canSlowTime = false;
               
            }
            else if(!_needCoolDown && Time.time > nextTimeCanStop)
            {
                canSlowTime = true;
            }
            
        
    }

    public enum GameStates { NormalTime, SlowTime, CoolTime};
}
