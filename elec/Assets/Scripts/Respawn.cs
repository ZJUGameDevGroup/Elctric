using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("1");
        if(collision.tag == "Player")
        {
            collision.transform.position = transform.position;
        }
    }
}
