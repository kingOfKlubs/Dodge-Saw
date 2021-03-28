using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link {
    public GameObject _link;
    public Connector _connector;
    public string targetName;
}

public class Detector : MonoBehaviour
{
    public GameObject _linkPrefab;

    public List<Link> _linkList = new List<Link>();

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Lightning Rod")
        {
            Debug.Log("Entered Lightning Rod");
            if(_linkPrefab != null)
            {
                Link newLink = new Link() {_link = Instantiate(_linkPrefab) as GameObject};
                newLink._connector = newLink._link.GetComponent<Connector>();
                newLink.targetName = col.gameObject.name;
                _linkList.Add(newLink);
                if(newLink._connector != null)
                {
                    newLink._connector.MakeConnection(transform.position, col.transform.position);
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(_linkList.Count > 0)
        {
            for (int i = 0; i < _linkList.Count; i++)
            {
                if(col.name == _linkList[i].targetName)
                {
                    _linkList[i]._connector.MakeConnection(transform.position, col.transform.position);
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (_linkList.Count > 0)
        {
            for (int i = 0; i < _linkList.Count; i++)
            {
                if (col.name == _linkList[i].targetName)
                {
                    Destroy(_linkList[i]._link);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

