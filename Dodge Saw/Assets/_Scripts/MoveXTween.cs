using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXTween : MonoBehaviour
{
    public LeanTweenType easeType;
    public float _moveXValue;
    public float _duration;

    // Start is called before the first frame update
    private void OnEnable()
    {
        LeanTween.moveLocalX(gameObject, _moveXValue, _duration).setEase(easeType);
    }
}
