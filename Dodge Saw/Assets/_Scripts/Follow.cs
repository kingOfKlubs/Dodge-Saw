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
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GetBehindPosition(target);
    }
    Vector2 GetBehindPosition(Transform target)
    {
        return new Vector2(target.position.x,target.position.y) - (target.GetComponent<Movement>()._direction * offset);
    }
}
