using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : EnemyMovement
{

    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        Reflect();
        Move();
    }

    private void Reflect()
    {
        Ray _ray;
        _ray = new Ray(transform.position, _moveDirection);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            _rigid.velocity = Vector2.Reflect(_moveDirection, _hit.normal);
            _moveDirection = _rigid.velocity;
        }

    }

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void Move()
    {
        base.Move();
    }
}
