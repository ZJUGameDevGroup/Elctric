using UnityEngine;
using System.Collections;
using System;
public class MoveMent : MonoBehaviour
{
    //物理，碰撞和动画组件
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    //速度，跳跃力度
    public float speed;
    // public float jumpForce;
    private float horizontalMove;
    private float verticalMove;
    [Header("地面检测")]
    //检测地面
    public Transform groundCheck;
    public LayerMask ground;
    public float checkGroundRadius;
    [Header("Dash参数")]
    public float dashUDSpeed;
    public float dashLRSpeed;
    public float dashTime;//dash时长
    private float dashTimeLeft;//冲锋剩余时间
    private float lastDash = -10f;//上一次dash时间点
    public float dashCoolDown;
    // public float dashSpeed;
    private bool dashable;
    [Header("Attach")]
    [ReadOnly]
    public bool attachTo = false;
    public float checkRadius;
    float originVelocity;
    ElecItem attachedObject;
    [Header("Movement")]
    public bool isGround, isDashing;
    // public bool isJump;
    // bool jumpPressed;
    // int jumpCount;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {

        // if (Input.GetButtonDown("Jump") && jumpCount > 0)
        // {
        //     jumpPressed = true;
        // }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (dashable && Time.time >= (lastDash + dashCoolDown))
            {
                //可以执行dash
                ReadyToDash();
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Debug.Log("Z");
            Collider2D nearest;
            Collider2D[] Items = Physics2D.OverlapCircleAll(transform.position, checkRadius, LayerMask.GetMask("ElecItem"));
            if (Items.Length > 0)
            {
                Debug.Log("Has");
                nearest = getNearestIndex(transform.position, Items);
                attachedObject = nearest.GetComponent<ElecItem>();
                if (attachedObject != null)
                {
                    Debug.Log("Enter");
                    attachedObject.ChangeChaged();
                    StartCoroutine(EnterItem(nearest));
                }
            }
        }

    }
    IEnumerator EnterItem(Collider2D Item)
    {
        int i = 0, totalFrame = 15;
        float scale;
        if (!attachTo)
        {
            attachTo = true;
            originVelocity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            while (i < totalFrame)
            {
                i++;
                transform.position += (Item.transform.position - transform.position) / totalFrame;
                scale = 1f - i / (float)totalFrame;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            attachTo = false;
            rb.gravityScale = originVelocity;
            while (i < totalFrame)
            {
                i++;
                scale = i / (float)totalFrame;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return new WaitForFixedUpdate();
            }
        }

    }

    private Collider2D getNearestIndex(Vector2 centerPos, Collider2D[] Items)
    {
        Collider2D nearest = Items[0];
        float distance = Vector2.Distance(centerPos, Items[0].transform.position);
        float newDistance;
        for (int i = 0; i < Items.Length; i++)
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
    void ReadyToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }
    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, ground);
        if (isGround)
        {
            dashable = true;
        }
        if (!attachTo)
        {
            GroundMovement();
            // Jump();
        }
        else
        {
            MoveTogether();
        }
        Dash();
    }
    void MoveTogether()
    {
        Vector3 newTranslate;
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if (attachedObject.moveDirection == ElecItem.Direction.UD)
        {
            newTranslate = new Vector3(0, verticalMove, 0);
            transform.Translate(newTranslate);
            attachedObject.transform.Translate(newTranslate);
            float y = Mathf.Clamp(transform.position.y, attachedObject.beginTransform.position.y,
            attachedObject.endTransform.position.y);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            y = Mathf.Clamp(attachedObject.transform.position.y, attachedObject.beginTransform.position.y,
            attachedObject.endTransform.position.y);
            attachedObject.transform.position = new Vector3(attachedObject.transform.position.x, y, attachedObject.transform.position.z);

        }
        else
        {
            newTranslate = new Vector3(horizontalMove, 0, 0);
            transform.Translate(newTranslate);
            attachedObject.transform.Translate(newTranslate);
            float x = Mathf.Clamp(transform.position.x, attachedObject.beginTransform.position.x,
            attachedObject.endTransform.position.x);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            x = Mathf.Clamp(attachedObject.transform.position.x, attachedObject.beginTransform.position.x,
            attachedObject.endTransform.position.x);
            attachedObject.transform.position = new Vector3(x, attachedObject.transform.position.y, attachedObject.transform.position.z);
        }

    }
    void Dash()
    {
        if (isDashing)
        {
            if (attachedObject != null && attachTo)
            {
                attachedObject.ChangeChaged();
                Collider2D nearest = attachedObject.GetComponent<Collider2D>();
                StartCoroutine(EnterItem(nearest));
                attachedObject = null;
            }
            if (dashTimeLeft > 0)
            {

                // if (!isGround)
                // {
                //     rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce * verticalMove);//在空中Dash向上
                // }
                // else if(isGround)
                // {
                //     rb.velocity = new Vector2(dashSpeed * (horizontalMove + gameObject.transform.localScale.x) / 2, rb.velocity.y);//地面Dash
                // }

                rb.velocity = new Vector2(dashLRSpeed * horizontalMove, dashUDSpeed * verticalMove);//在空中Dash向上

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                dashable = false;
                // if (!isGround)
                // {
                //     //目的为了在空中结束 Dash 的时候可以接一个小跳跃。根据自己需要随意删减调整
                //     rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                // }
            }
        }

    }
    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);

        }
    }
    // void Jump()//跳跃
    // {
    //     if (isGround)
    //     {
    //         jumpCount = 2;//可跳跃数量
    //         isJump = false;
    //     }
    //     if (jumpPressed && isGround)
    //     {
    //         isJump = true;
    //         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //         jumpCount--;
    //         jumpPressed = false;
    //     }
    //     else if (jumpPressed && jumpCount > 0 && isJump)
    //     {
    //         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //         jumpCount--;
    //         jumpPressed = false;
    //     }
    // }
}
