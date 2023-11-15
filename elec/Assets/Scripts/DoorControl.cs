using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    Collider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        transform.localScale = new Vector3(4, 1, 1);
        transform.localPosition = new Vector3(45,transform.localPosition.y,transform.localPosition.z);
        collider2D.enabled = false;
    }
}
