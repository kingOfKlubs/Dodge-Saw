using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody2D _rigid;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float Xspeed = Input.GetAxis("Horizontal");
        float Yspeed = Input.GetAxis("Vertical");

        _rigid.velocity = new Vector2(Xspeed, Yspeed);
    }
}
