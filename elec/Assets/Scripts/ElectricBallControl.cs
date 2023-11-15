using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class ElectricBallControl : MonoBehaviour
{
    public PhysicsMaterial2D p1;
    public PhysicsMaterial2D p2;

    public float speed = 7.5f;
    int faceDir = 1;//-1 for left, 1 for right
    public float checkRadius = 1f;

    Rigidbody2D rb;

    float InputX;
    float InputY;

    bool isEntering = false;
    bool isGrounded = true;
    bool canDash = true;
    bool Moveable = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        if (isGrounded)
        {
            rb.sharedMaterial = p1;
        }
        else
        {
            rb.sharedMaterial = p2;
        }

        if (Moveable)
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space) && (canDash || isEntering))
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Collider2D nearest;
            Collider2D[] Items = Physics2D.OverlapCircleAll(transform.position, checkRadius, LayerMask.GetMask("ElecItem"));
            if (Items.Length > 0)
            {
                nearest = getNearestIndex(transform.position, Items);
                ElecItem Item = nearest.GetComponent<ElecItem>();
                Item.Entered();
                StartCoroutine(EnterItem(nearest));
            }
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded)
        {
            isGrounded = true;
            canDash = true;
        }
        else
        {
            isGrounded= false;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(InputX * speed, rb.velocity.y);
        if(InputX > 0)
        {
            faceDir = 1;
        }
        else if(InputX < 0)
        {
            faceDir = -1;
        }
    }

    IEnumerator Dash()
    {
        Vector2 dashDir;
        float dashSpeed = 0.4f,scale;
        int i = 0,totalFrame = 9;
        if(InputX == 0)
        {
            dashDir.x = 0;
        }
        else
        {
            dashDir.x = Mathf.Sign(InputX);
        }
        if(InputY == 0)
        {
            dashDir.y = 0;
        }
        else
        {
            dashDir.y = Mathf.Sign(InputY);
        }
        if(dashDir == Vector2.zero)
        {
            dashDir.x = faceDir;
        }
        if (isEntering)
        {
            Moveable = true;
            isEntering = false;
            rb.gravityScale = 1;
            while(i < totalFrame)
            {
                i++;
                scale = i / (float)totalFrame;
                transform.localScale = new Vector3(scale, scale, scale);
                rb.MovePosition(rb.position + dashDir * dashSpeed);
                canDash = false;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (i < 9)
            {            
                rb.MovePosition(rb.position + dashDir * dashSpeed);
                i++;
                canDash = false;
                yield return new WaitForFixedUpdate();
            }
        }
        

    }
    IEnumerator EnterItem(Collider2D Item)
    {
        int i = 0,totalFrame = 15;
        float scale;
        if (!isEntering)
        {
            Moveable = false;
            isEntering = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            while(i < totalFrame)
            {
                i++;
                transform.position += (Item.transform.position - transform.position) / totalFrame;
                scale = 1f - i / (float)totalFrame;
                transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            Moveable = true;
            isEntering = false;
            rb.gravityScale = 1;
            while (i < totalFrame)
            {
                i++;
                scale = i / (float)totalFrame;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return new WaitForFixedUpdate();
            }
        }
        
    }

    private Collider2D getNearestIndex(Vector2 centerPos,Collider2D[] Items)
    {
        Collider2D nearest = Items[0];
        float distance = Vector2.Distance(centerPos,Items[0].transform.position);
        float newDistance;
        for(int i = 0;i < Items.Length; i++)
        {
            newDistance = Vector2.Distance(centerPos, Items[0].transform.position);
            if (newDistance < distance)
            {
                nearest = Items[i];
                distance = newDistance;
            }
        }

        return nearest;
    }


}
