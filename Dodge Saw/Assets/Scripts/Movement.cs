using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody2D _rigid;
    Vector2 velocity;
    public float dst;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        velocity = new Vector2(1,1);
        direction = this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
        //float Xspeed = Input.GetAxis("Horizontal");
        //float Yspeed = Input.GetAxis("Vertical");

        //_rigid.velocity = new Vector2(Xspeed, Yspeed);
            _rigid.velocity = velocity;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f))
        {
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0f, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
        }
            Reflect();
        Debug.Log(velocity);
    }
    private void Reflect()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
            if(Physics.Raycast(ray, out hit, dst))
            {
                velocity = Vector2.Reflect(direction, hit.normal);
                direction = velocity;
            }
            
    }

 

}
