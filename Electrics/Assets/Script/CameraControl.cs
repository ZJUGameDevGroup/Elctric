using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float followSpeed = 2.0f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private Transform PlayerTransform;
    private Vector2 targetPosition;
 
    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = PlayerTransform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
    void LateUpdate()
    {
        if (PlayerTransform != null)
        {
            targetPosition.x = Mathf.Clamp(PlayerTransform.position.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(PlayerTransform.position.y, minY, maxY);
            transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}
