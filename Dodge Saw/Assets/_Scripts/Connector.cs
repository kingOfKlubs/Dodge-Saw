using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

    public LineRenderer _lineRenderer;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool makeConnection;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(makeConnection == true)
        {
            _lineRenderer.SetPosition(0, _startPos);
            _lineRenderer.SetPosition(1, _endPos);
        }

    }

    public void MakeConnection(Vector3 startPos, Vector3 endPos)
    {
        _startPos = startPos;
        _endPos = endPos;

        makeConnection = true;
    }

}
