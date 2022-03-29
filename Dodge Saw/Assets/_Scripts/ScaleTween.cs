using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTween : MonoBehaviour
{
    public LeanTweenType easeType;
    public Vector3 _endScale;
    public float _duration;

    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, _endScale, _duration).setEase(easeType);
    }
}
