using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
            AudioManager.instance.Play("EnemyBump");
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

    public override void Death()
    {
        Vector4 BaseColor = GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        base.Death();
    }
}
