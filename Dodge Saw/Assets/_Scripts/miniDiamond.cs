using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class miniDiamond : MonoBehaviour
{
    public GameObject _playerDeathEffect;
    public VisualEffect _destroyEffect;
    public float time; //should be half of what ever lifeTime for SplitMove is
    Rigidbody _rigid;
    public Vector2 _moveDirection;
    public FindingDimensions findingDimensions = new FindingDimensions();
    Vector2 _position;
    Vector2 bottomRange;
    Vector2 topRange;
    protected GameObject _player;
    protected Animator anim;
    protected Touch touch;
    protected float _moveSpeed;



    // Start is called before the first frame update
    void Start()
    {
        //_rigid = GetComponent<Rigidbody>();
        //_position = new Vector2(Random.Range(bottomRange.x + findingDimensions.padding, topRange.x - findingDimensions.padding), Random.Range(bottomRange.y + findingDimensions.padding, topRange.y - findingDimensions.padding));
        //_moveDirection = (new Vector3(_position.x,_position.y, 0) - transform.position).normalized;
        anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
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
        DeleteOutOfBounds();
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

    public virtual void Death()
    {

        Vector4 BaseColor = GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        VisualEffect deathClone = Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Destroy(deathClone.gameObject, 2);
        Destroy(this.gameObject);
    }

    public void DeleteOutOfBounds() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f)) {
            Death();
        }
    }
}
