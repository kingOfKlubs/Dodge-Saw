using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTween : MonoBehaviour
{
    public LeanTweenType easeType;
    public float moveDistance;
    public float moveTime;

    // Start is called before the first frame update
    private void OnEnable() {
        //transform.localScale = new Vector3(0, 0, 0);
        //LeanTween.scale(gameObject, new Vector3(1, 1, 1), .7f).setEase(easeType);
        LeanTween.moveLocalX(gameObject, moveDistance, moveTime).setLoopPingPong(3).setOnComplete(OnComplete);
    }

    public void OnComplete() {
        gameObject.SetActive(false);
    }
}
