using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveYTween : MonoBehaviour
{
    public LeanTweenType easeType;
    public float _moveYValue; 
    public float _duration;
    public float startTime;
    public int numOfLoops;

    float time;

    private void OnEnable()
    {
        time = startTime;
        LeanTween.moveLocalY(gameObject, _moveYValue, _duration).setEase(easeType).setLoopPingPong(numOfLoops);
    }

    //private void FixedUpdate()
    //{
    //    time -= Time.deltaTime;
    //    if(time <= 0)
    //    {
    //        LeanTween.moveLocalY(gameObject, _moveYValue, _duration).setLoopCount(numOfLoops).setEase(easeType);
    //        time = startTime;
    //    }
    //}
}
