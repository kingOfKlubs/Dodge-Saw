using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = GetBehindPosition(target);
            this.transform.LookAt(target);
        }
    }
    Vector2 GetBehindPosition(Transform target)
    {
        return new Vector2(target.position.x,target.position.y) + (target.GetComponent<Movement>()._direction * offset);
    }
}
