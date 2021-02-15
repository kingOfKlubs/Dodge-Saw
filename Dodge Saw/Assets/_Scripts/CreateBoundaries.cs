using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoundaries : MonoBehaviour
{

    public GameObject topRightBoundary;
    public GameObject bottomLeftBoundary;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        SetUpBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUpBounds()
    {
        topRightBoundary.transform.position = GetWorldPosition(0, new Vector2(Screen.width, Screen.width));
        bottomLeftBoundary.transform.position = GetWorldPosition(0, new Vector2(0, 0));
    }

    private Vector3 GetWorldPosition(float z, Vector2 screenPoint)
    {
        Ray Point = camera.ScreenPointToRay(screenPoint);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(Point, out distance);
        return Point.GetPoint(distance);
    }
}

