using UnityEngine;

public class ChangeOffset : MonoBehaviour
{
    public Follow follow;
    public float offsetOverride;

    // Start is called before the first frame update
    void Start()
    {
        follow = GetComponentInParent<Follow>();
        follow.offset = offsetOverride;
    }
}
