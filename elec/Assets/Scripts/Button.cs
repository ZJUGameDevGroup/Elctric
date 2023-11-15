using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject Door;
    DoorControl doorControl;
    // Start is called before the first frame update
    void Start()
    {
        doorControl = Door.GetComponent<DoorControl>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        doorControl.Open();
        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, 0.03f, transform.GetChild(0).localPosition.z);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, 0.26f, transform.GetChild(0).localPosition.z);
    }
}
