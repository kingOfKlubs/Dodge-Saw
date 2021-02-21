using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeMove : EnemyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        MovePerpendicular();
        Move();
    }

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void Move()
    {
        base.Move();
    }

    private void MovePerpendicular()
    {
        Ray _ray;
        _ray = new Ray(transform.position, _moveDirection);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            Debug.DrawLine(transform.position, _ray.direction);
            _rigid.velocity = Align(_moveDirection, _hit.normal);
            float angle = Vector2.Angle(_moveDirection, _hit.normal);
            _moveDirection = _rigid.velocity;
            Debug.Log("_moveDirection is " + _moveDirection);
        }
    }

    private void OnDrawGizmos()
    {
        Ray _ray;
        _ray = new Ray(transform.position, new Vector2(transform.position.x, transform.position.y) + _moveDirection);
        RaycastHit _hit = new RaycastHit();
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            Gizmos.DrawLine(transform.position, _moveDirection);
        }
    }

    public Vector3 Align(Vector3 vector, Vector3 normal)
    {
        //typically used to rotate a movement vector by a surface normal
        Vector3 tangent = Vector3.Cross(normal, vector);
        Vector3 newVector = -Vector3.Cross(normal, tangent);
        vector = newVector.normalized * vector.magnitude;
        return vector;
    }
}
