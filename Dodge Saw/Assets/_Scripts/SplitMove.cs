using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMove : EnemyMovement
{
    public GameObject miniDiamond;
    public int radius;

    private bool calledOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckWall();
        if (!calledOnce)
        {
            StartCoroutine(TimeToSplit());
            calledOnce = true;
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

    public void CheckWall()
    {
        Ray _ray;
        _ray = new Ray(transform.position, _moveDirection);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, _dst, layer))
        {
            SplitOff();
        }
    }

    public void SplitOff()
    {
        float angleStep = 360f / 2;
        float angle = Vector2.Angle(transform.position, _player.transform.position);

        for (int i = 0; i <= 2 - 1; i++)
        {

            float projectileDirXposition = this.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYposition = this.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 pos = transform.position;
            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - pos).normalized * 5; // this value is equal to _moveSpeed but has to be one value instead so projectiles don't launch slowly

            Debug.Log("projectileMoveDirection is " + projectileMoveDirection);

            var proj = Instantiate(miniDiamond, pos, Quaternion.identity);
            //proj.GetComponent<Rigidbody>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            proj.GetComponent<miniDiamond>()._moveDirection = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y).normalized;

            angle += angleStep;
        }

        Destroy(this.gameObject);
    }

    public override void Death()
    {
        Vector4 BaseColor =  GetComponent<MeshRenderer>().sharedMaterials[0].GetVector("_EmissionColor");
        Vector4 Sparks = GetComponent<MeshRenderer>().sharedMaterials[1].GetVector("_EmissionColor");

        _destroyEffect.SetVector4("Base Color", BaseColor);
        _destroyEffect.SetVector4("Sparks", Sparks);
        base.Death();
    }

    IEnumerator TimeToSplit()
    {
        yield return new WaitForSeconds(1);
        SplitOff();
    }
}
