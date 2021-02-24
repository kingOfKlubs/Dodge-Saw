using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectMove : EnemyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
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

    public override void Death()
    {
        Vector4 BaseColor = GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        base.Death();
    }


}
