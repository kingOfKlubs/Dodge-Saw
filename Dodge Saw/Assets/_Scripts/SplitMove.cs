using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMove : EnemyMovement
{
    public GameObject miniDiamond;
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        SplitOff();
    }

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void Move()
    {
        base.Move();
    }

    public void SplitOff()
    {
        Ray _ray;
        _ray = new Ray(transform.position, _moveDirection);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            GameObject DiamondClone1 = Instantiate(miniDiamond, transform.position, Quaternion.identity);
            GameObject DiamondClone2 = Instantiate(miniDiamond, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public override void Death()
    {
        Vector4 BaseColor =  GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        base.Death();
    }
}
