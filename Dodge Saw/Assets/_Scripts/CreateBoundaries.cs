using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoundaries : MonoBehaviour
{

    public GameObject topRightBoundary;
    public GameObject bottomLeftBoundary;
    public FindingDimensions findingDimensions = new FindingDimensions();

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
        topRightBoundary.transform.position = findingDimensions.GetWorldPosition(0, new Vector2(Screen.width, Screen.height));
        bottomLeftBoundary.transform.position = findingDimensions.GetWorldPosition(0, new Vector2(0, 0));
    }
}

public class FindingDimensions {

    public float padding = 1;

    public Vector3 GetWorldPosition(float z, Vector2 screenPoint) {
        Ray Point = Camera.main.ScreenPointToRay(screenPoint);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(Point, out distance);
        return Point.GetPoint(distance);
    }
}

