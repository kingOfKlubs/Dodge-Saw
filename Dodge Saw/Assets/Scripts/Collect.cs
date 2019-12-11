using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public int _cost;
    public ParticleSystem collect;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Score._score += _cost;
            this.gameObject.SetActive(false);
            ParticleSystem clone = Instantiate(collect, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("Coin");
            Destroy(clone, 3);
        }
    }
}
