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
    public float _distance;
    public static bool Death;
    public ParticleSystem Warp;
    public ParticleSystem Warp1;
    public ParticleSystem Warp2;
    public ParticleSystem Warp3;
    public float _coolDownTime = -5;
    public Image _coolDownImage;
    public Image _coolDownImageLarge;
    public float _startTime = 0;
    public LayerMask border;
    #endregion

    #region Private Variables
    Vector2 _velocity;
    Rigidbody _rigid;
    Vector2 _direction;
    Vector2 _initialPos;
    Vector2 _endPos;
    Ray _ray;
    float _initialSpeed;
    float _angle;
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
        _rigid = GetComponent<Rigidbody>();
        _velocity = new Vector2(0, 1);
        _direction = this.transform.up;
        _initialSpeed = _moveSpeed;
        anim = GetComponent<Animator>();
        _timeStopped = _startTime;
        _coolDownImage.gameObject.SetActive(true);
        _coolDownImageLarge.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _rigid.velocity = _velocity * _moveSpeed;
        KeepInBounds();
        Reflect();
        Move();
        TestingMovement();
        CoolDown();
        SwitchStates();

        
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
        if (Physics.Raycast(_ray, out _hit, _dst,border))
        {
            _velocity = Vector2.Reflect(_direction, _hit.normal);
            _direction = _velocity;
		    FindObjectOfType<AudioManager>().Play("Bump");
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

        //TODO find a way to collect all of the particles on screen without dragging from the inspector
        Warp.playbackSpeed = 1;
        Warp1.playbackSpeed = 1;
        Warp2.playbackSpeed = 1;
        Warp3.playbackSpeed = 1;
        
        _coolDownImage.fillAmount = 1;
        _coolDownImageLarge.fillAmount = 1;
        _coolDownImage.gameObject.SetActive(false);
        _coolDownImageLarge.gameObject.SetActive(false);
        


    }

    public void MoveCool()
    {
        _timeStopped = _startTime;
        _moveSpeed = _initialSpeed;
        anim.speed = 1;
        Warp.playbackSpeed = 1;
        Warp1.playbackSpeed = 1;
        Warp2.playbackSpeed = 1;
        Warp3.playbackSpeed = 1;
        float counter = 5;
        counter -= Time.deltaTime;
        _coolDownImage.fillAmount += 1 / _coolDownTime * Time.deltaTime;
        _coolDownImageLarge.fillAmount += 1 / _coolDownTime * Time.deltaTime;
        _coolDownImage.gameObject.SetActive(true);
        _coolDownImageLarge.gameObject.SetActive(false);

    }



    public void MoveSlow()
    {
        _coolDownImage.fillAmount -= 1 / _startTime * Time.deltaTime;
        _coolDownImageLarge.fillAmount -= 1 / _startTime * Time.deltaTime;
        _coolDownImage.gameObject.SetActive(false);
        Debug.Log("this line was called");
        _coolDownImageLarge.gameObject.SetActive(true);
        _moveSpeed = .3f;
        anim.speed = .3f;
        Warp.playbackSpeed = .3f;
        Warp1.playbackSpeed = .3f;
        Warp2.playbackSpeed = .5f;
        Warp3.playbackSpeed = .5f;
        //FindObjectOfType<AudioManager>().Play("");
    }
    //moves the pentagon based on touch input
    public void Move()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            TouchPhase phase = touch.phase;
            _timeStopped -= Time.deltaTime;


            if (_needCoolDown == false && _timeStopped > 0 && canSlowTime)
                UpdateStates(GameStates.SlowTime);
            else if (_needCoolDown == false && _timeStopped <= 0)
            {
                UpdateStates(GameStates.CoolTime);
                _needCoolDown = true;
            }
            switch(phase)
            {
                case TouchPhase.Began:
                    _initialPos = touch.position;
                    //Debug.Log("the initial position is " + _initialPos);
                    break;
                case TouchPhase.Ended:
                    _endPos = touch.position;
                   // Debug.Log("the end position is " + _endPos);
                    break;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeDir = _initialPos - _endPos;
                _angle = Vector2.SignedAngle(Vector2.down, swipeDir);

                _distance = Vector2.Distance(_initialPos, _endPos);
                //Debug.Log("Calculated distance is " + distance);

                if(_distance >= 150)
                {

                    if (_angle < 120 && _angle > 60)// move left
                    {
                        _velocity = new Vector2(-1, 0);
                        _direction = _velocity;
                    }
                    else if (_angle < 60 && _angle > 30)//move left and up  
                    {
                        _velocity = new Vector2(-1, 1);
                        _direction = _velocity;
                    }
                    else if (_angle < 30 && _angle > -30)//move up
                    {
                        _velocity = new Vector2(0, 1);
                        _direction = _velocity;
                    }
                    else if (_angle < -30 && _angle > -60)//move right and up  
                    {
                        _velocity = new Vector2(1, 1);
                        _direction = _velocity;
                    }
                    else if (_angle < -60 && _angle > -120)//move right
                    {
                        _velocity = new Vector2(1, 0);
                        _direction = _velocity;
                    }
                    else if (_angle < -120 && _angle > -150)//move right and down 
                    {
                        _velocity = new Vector2(1, -1);
                        _direction = _velocity;
                    }
                    else if (_angle < -150 || _angle > 150)//move down
                    {
                        _velocity = new Vector2(0, -1);
                        _direction = _velocity;
                    }
                    else if (_angle < 150 && _angle > 120)//move left and down 
                    {
                        _velocity = new Vector2(-1, -1);
                        _direction = _velocity;
                    }
                }
            }
        }
        else if (canSlowTime == false)
        {
            UpdateStates(GameStates.CoolTime);
        }
        else
        {
            UpdateStates(GameStates.NormalTime);
        }
    }

    void TestingMovement()
    {
        if (Input.GetKeyDown("a"))//move left
        {
            _velocity = new Vector2(-1, 0);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("a") && Input.GetKeyDown("w"))//move left and up  
        {
            _velocity = new Vector2(-1, 1);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("w"))//move up
        {
            _velocity = new Vector2(0, 1);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("w") && Input.GetKeyDown("d"))//move right and up  
        {
            _velocity = new Vector2(1, 1);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("d"))//move right
        {
            _velocity = new Vector2(1, 0);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("s") && Input.GetKeyDown("d"))//move right and down 
        {
            _velocity = new Vector2(1, -1);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("s"))//move down
        {
            _velocity = new Vector2(0, -1);
            _direction = _velocity;
        }
        else if (Input.GetKeyDown("w") && Input.GetKeyDown("a"))//move left and down 
        {
            _velocity = new Vector2(-1, -1);
            _direction = _velocity;
        }
    }

    void CoolDown()
    {

        if (_needCoolDown && Time.time > nextTimeCanStop)
        {
            nextTimeCanStop = Time.time + _coolDownTime;
            _needCoolDown = false;
            canSlowTime = false;

        }
        else if (!_needCoolDown && Time.time > nextTimeCanStop)
        {
            canSlowTime = true;
        }


    }

    public enum GameStates { NormalTime, SlowTime, CoolTime};
}
