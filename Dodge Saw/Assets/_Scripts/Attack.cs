using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyMovement
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
}
