using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectMove : EnemyMovement
{
    Vector3 player;
    Quaternion targetRotation;
    Movement playerMovement;
    int tries = 3;

    // Start is called before the first frame update
    void Start()
    {
        LookAtPlayer();
        Initiate();
        tries = 3;
        playerMovement = FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(playerMovement.currentState == Movement.GameStates.SlowTime)
        {
            LookAtPlayer();
            Redirect();        
        }
        DeleteOutOfBounds();
    }

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void Move()
    {
        base.Move();
    }

    public void Redirect()
    {
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

    public override void Death()
    {
        Vector4 BaseColor = GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        base.Death();
    }

    public void LookAtPlayer()
    {
        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.LookAt(player);
        }
    }

    public void DeleteOutOfBounds() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f)) {
            Debug.Log("should be calling death here");
            Death();
        }
    }
}
