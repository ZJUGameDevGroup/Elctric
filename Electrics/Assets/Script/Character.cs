using System;
using System.Collections;
using UnityEngine;
class Character : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D coll;
    [Header("角色属性")]
    //移动速度
    public float moveSpeed;
    //上下冲刺速度
    public float DashUDSpeed;
    //左右冲刺速度
    public float DashLRSpeed;
    //冲刺时间
    public float dashTime;

    //状态机
    Context context;

    private void Awake()
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            //取消阻力
            rb.drag = 0;
            //取消重力
            rb.gravityScale = 0;
        }
        if (coll == null)
        {
            coll = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        context = new Context(new OnGround());
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        context.Handler(this);
    }
}
abstract class Actor
{
    public abstract void Act(Character character);
}
class MoveActor : Actor
{
    private float horizontal;
    private float vertical;
    private float horizontalSpeed;
    private float verticalSpeed;
    public MoveActor(float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        this.horizontalSpeed = horizontalSpeed;
        this.verticalSpeed = verticalSpeed;
    }
    public override void Act(Character character)
    {
        //改变速度
        character.rb.velocity = new Vector2(horizontal * horizontalSpeed, vertical * verticalSpeed);
        //改变朝向
        if (horizontal != 0)
        {
            character.transform.localScale = new Vector3(horizontal, 1, 1);
        }
    }
}

class ForceActor : Actor
{
    private float rightForce;
    private float upForce;
    private Vector2 right;
    private Vector2 up;
    public ForceActor(Vector2 right, float rightForce, Vector2 up, float upForce)
    {
        this.up = up;
        this.right = right;
        this.upForce = upForce;
        this.rightForce = rightForce;
    }
    public override void Act(Character character)
    {
        //施加外力
        character.rb.AddRelativeForce(rightForce * right + upForce * up);
        //改变朝向
        if (right != Vector2.zero)
        {
            character.transform.localScale = new Vector3(right.x, right.y, 1);
        }
    }
}

abstract class Command
{
    public abstract void Handle(Character character);
}
class MoveCommand : Command
{
    Actor moveActor;
    public MoveCommand(float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        moveActor = new MoveActor(horizontal, horizontalSpeed, vertical, verticalSpeed);
    }
    public override void Handle(Character character)
    {
        moveActor.Act(character);
    }
}
class ForceCommand : Command
{
    Actor forceActor;
    public ForceCommand(Vector2 right, float rightForce, Vector2 up, float upForce)
    {
        forceActor = new ForceActor(right, rightForce, up, upForce);
    }
    public override void Handle(Character character)
    {
        forceActor.Act(character);
    }
}
class Context
{
    public State state;
    public State State
    {
        get => state;
        set => state = value;
    }
    public Context(State state)
    {
        this.state = state;
    }
    public void Enter(Character character)
    {
        state.Enter(character);
    }
    public void Handler(Character character)
    {
        State _state = state.Handler(character);
        if (_state != null)
        {
            state.Leave(character);
            state = _state;
            state.Enter(character);
        }
    }
    public void Leave(Character character)
    {
        state.Leave(character);
    }
}
abstract class State
{
    public abstract void Enter(Character character);
    public abstract State Handler(Character character);
    public abstract void Leave(Character character);
}
class UnAttached : State
{
    float checkRadius = 15.0f;
    string checkMask = "EleItem";
    public override void Enter(Character character)
    {

    }
    public override State Handler(Character character)
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Debug.Log("UnAttached.Handler.Z");
            return CheckAttachableItem(character);
        }
        return null;
    }
    public override void Leave(Character character)
    {

    }
    private State CheckAttachableItem(Character character)
    {
        Collider2D nearest;
        Collider2D[] Items = Physics2D.OverlapCircleAll(character.transform.position, checkRadius, LayerMask.GetMask(checkMask));
        if (Items.Length > 0)
        {
            nearest = getNearestIndex(character.transform.position, Items);
            GameObject attachedObject = nearest.GetComponent<GameObject>();
            return new Attached(attachedObject);
        } 
        return null;
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
}
class Attached : State
{
    public GameObject attachedObject;
    float originGravityScale;
    int countFrames;
    float delta;
    Vector3 deltaScale;

    public Attached(GameObject gameObject)
    {
        attachedObject = gameObject;
        countFrames = 15;
        delta = 1.0f / (float)countFrames;
        deltaScale = new Vector3(delta, delta, delta);
    }
    public override void Enter(Character character)
    {
        //记录重力
        originGravityScale = character.rb.gravityScale;
        //重力置为0
        character.rb.gravityScale = 0.0f;
        //速度置为0
        character.rb.velocity = Vector2.zero;
        //播放进入动画
        character.StartCoroutine(EnterAnimation(character));

    }
    public override State Handler(Character character)
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            return new UnAttached();
        }
        return null;
    }
    public override void Leave(Character character)
    {
        //恢复重力
        character.rb.gravityScale = originGravityScale;
        //播放退出动画
        character.StartCoroutine(LeaveAnimation(character));
    }
    private IEnumerator EnterAnimation(Character character)
    {
        Debug.Log("EnterAnimation");
        Vector3 deltaDistance = (attachedObject.transform.position - character.transform.position) / countFrames;
        for (int i = 0; i < countFrames; i++)
        {
            character.transform.position += deltaDistance;
            character.transform.localScale -= deltaScale;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator LeaveAnimation(Character character)
    {
        Debug.Log("LeaveAnimation");
        for (int i = 0; i < countFrames; i++)
        {
            character.transform.localScale += deltaScale;
            yield return new WaitForFixedUpdate();
        }
    }
}
class OnGround : UnAttached
{
    public float horizontal;
    public float vertical;
    public override void Enter(Character character)
    {
        base.Enter(character);
    }
    public override State Handler(Character character)
    {
        State state = base.Handler(character);
        if (state != null)
        {
            return state;
        }
        // Debug.Log("OnGround.Handler");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (horizontal == 0 && vertical == 0)
        {
            return new StandOnGround();
        }
        else if (vertical == 0 && horizontal != 0)
        {
            return new MoveOnGround();
        }
        return null;
    }
    public override void Leave(Character character)
    {
        base.Leave(character);
    }
}
class StandOnGround : OnGround
{
    public Command stopMoveCommand;
    public override void Enter(Character character)
    {
        base.Enter(character);
        stopMoveCommand = new MoveCommand(0.0f, 0.0f, 0.0f, 0.0f);
        stopMoveCommand.Handle(character);
    }
    public override State Handler(Character character)
    {
        State _state = base.Handler(character);
        if (_state != null)
        {
            return _state;
        }
        return null;
    }
    public override void Leave(Character character)
    {
        base.Leave(character);
    }
}
class MoveOnGround : OnGround
{

}
// class Dash : State
// {
//     public float horizontal;
//     public float vertical;
//     public Command dashCommand;
//     public override void Enter(Character character)
//     {
//         dashCommand = new MoveCommand(horizontal, character.DashLRSpeed, vertical, character.DashUDSpeed);
//         dashCommand.Handle(character);
//     }
//     public override State Handler(Character character)
//     {
//         // 更新残影
//         ShadowPool.instance.GetFromPool();
//         return null;
//     }
//     public override void Leave(Character character)
//     {

//     }
// }
