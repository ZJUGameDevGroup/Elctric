using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.transform.position.x,transform.position.y,transform.position.z);
        if(transform.position.x < 0)
        {
            transform.position = new Vector3(0,transform.position.y,transform.position.z);
        }
        if (transform.position.x > 38)
        {
            transform.position = new Vector3(38, transform.position.y, transform.position.z);
        }
    }
}
