using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDiagnol : MonoBehaviour
{

    Vector2 velocity;
    Rigidbody _rigid;
    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        velocity = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        _rigid.velocity = velocity * 5;
    }
}
