using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class HookControl : ElecItem
{
    SpriteRenderer spriteRenderer;
    SpriteLibrary spriteLibrary;

    Rigidbody2D rb;
    

    int currentState = -1;//-1 for close,1 for open
    bool haveBox = true;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        spriteLibrary = GetComponent<SpriteLibrary>();
        rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if(isEntered)
            {
                if(currentState == -1)
                {
                    currentState = 1;
                    spriteRenderer.sprite = spriteLibrary.GetSprite("Hook", "hook_open");
                    if (haveBox)
                    {
                        rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                        haveBox = false;
                    }
                }
                else
                {
                    spriteRenderer.sprite = spriteLibrary.GetSprite("Hook", "hook_close");
                    currentState = -1;
                }
            }
        }
    }

}
