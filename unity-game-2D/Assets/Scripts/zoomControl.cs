using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomControl : MonoBehaviour
{
    [SerializeField] float zoomSize=5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().orthographicSize = zoomSize;
    }
}
