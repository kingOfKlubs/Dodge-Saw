using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public int _cost;
    public ParticleSystem collectEffect;
    public Movement player;

    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Score._score += _cost;
            Score._scoreRecord += _cost;
            this.gameObject.SetActive(false);
            ParticleSystem clone = Instantiate(collectEffect, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("Coin");
            if(FindObjectOfType<TutorialManager>() != null)
            FindObjectOfType<TutorialManager>()._hasCollectedCoin = true;
            Destroy(this.gameObject, 3);
            Destroy(clone.gameObject, 3);
        }
    }

    public void Update()
    {
        if(player != null)
        {
            if(player.currentState == Movement.GameStates.SlowTime)
            {
                Animator anim = GetComponent<Animator>();
                anim.speed = .2f;
            }
            else
            {
                Animator anim = GetComponent<Animator>();
                anim.speed = 1f;
            }
        }
            
    }
}
