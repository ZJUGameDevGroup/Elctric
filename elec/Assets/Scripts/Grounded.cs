using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    public ElectricBallControl ball;
    BoxCollider2D boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        if(Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + boxCollider.offset, boxCollider.size, 0, LayerMask.GetMask("Ground")) != null)
        {
            ball.SetGrounded(true);
        }
        else
        {
            ball.SetGrounded(false);
        }
    }
}
