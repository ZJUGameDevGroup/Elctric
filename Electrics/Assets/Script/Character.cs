using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterImp", order = 1)]
public class CharacterImp : ScriptableObject
{
    [Header("人物移动属性")]
    public float dashXSpeed;
    public float dashYSpeed;
    public float dashTime;
    public float moveXSpeed;
}
public interface ICharacter : IMoveable, IForcible, IElecPower<IElecUser<ICharacter>>, IAttachTo<IAttachedBy<ICharacter>>
{
    Collider2D Collider2D
    {
        get; set;
    }
    Rigidbody2D Rigidbody2D
    {
        get; set;
    }
    Transform Transform
    {
        get;
    }
    CharacterImp CharacterImp
    {
        get; set;
    }
    Coroutine StartCoroutine(IEnumerator routine);
}
class Character : MonoBehaviour, ICharacter
{
    public Rigidbody2D rb;
    public Collider2D coll;
    public CharacterImp characterImp;
    private IAttachedBy<ICharacter> attachedItem;
    private IElecUser<ICharacter> elecUser;
    public Rigidbody2D Rigidbody2D
    {
        get => rb;
        set => rb = value;
    }
    public Collider2D Collider2D
    {
        get => coll;
        set => coll = value;
    }
    public Collider2D AttachableColl
    {
        get => coll;
        set => coll = value;
    }
    public Transform Transform
    {
        get => transform;
    }
    public CharacterImp CharacterImp
    {
        get => characterImp;
        set => characterImp = value;
    }
    public IAttachedBy<ICharacter> AttachedItem
    {
        get => attachedItem;
        set => attachedItem = value;
    }
    public IElecUser<ICharacter> ElecUser
    {
        get => elecUser;
        set => elecUser = value;
    }
    public void Move(float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        Debug.Log("Character Move");
        rb.velocity = new Vector2(horizontal * horizontalSpeed, vertical * verticalSpeed);
    }
    public void Force(Vector2 right, float rightForce, Vector2 up, float upForce)
    {
        Debug.Log("Character Force");
        rb.AddRelativeForce(rightForce * right + upForce * up);
    }
    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        Debug.Log("Character StartCoroutine");
        return base.StartCoroutine(routine);
    }
    public void SetAttachedItem(IAttachedBy<ICharacter> attachedItem)
    {
        Debug.Log("Character SetAttachedItem");
        AttachedItem = attachedItem;
    }
    public void SetElecUser(IElecUser<ICharacter> elecUser)
    {
        Debug.Log("Character SetAttachedItem");
        ElecUser = elecUser;
    }
    public void PowerOn()
    {
        Debug.Log("Character PowerOn");
        if (ElecUser != null)
        {
            ElecUser.Open();
        }
    }
    public void PowerOff()
    {
        Debug.Log("Character PowerOff");
        if (ElecUser != null)
        {
            ElecUser.Close();
        }
    }
}

class UnAttached : IState<ICharacter>
{
    public float checkOffset = 0.1f;
    private ICharacter character;
    private float horizontal;
    private float vertical;
    private Vector2 checkMaxRU;
    private Vector2 checkMaxLU;
    private Vector2 checkMaxRD;
    private Vector2 checkMinLU;
    private Vector2 checkMinLD;
    private Vector2 checkMinRD;
    public ICharacter BaseObject
    {
        get => character;
        set => character = value;
    }
    public UnAttached(ICharacter character)
    {
        BaseObject = character;
    }
    public void Enter()
    {
        Debug.Log("Enter UnAttached");
    }
    public IState<ICharacter> Handler()
    {
        Debug.Log("Handler UnAttached");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Z))
        {
            IAttachedBy<ICharacter> attachedItem = FindAttachableItem();
            if (attachedItem != null)
            {
                //建立连接
                BaseObject.SetAttachedItem(attachedItem);
                BaseObject.AttachedItem.SetAttachable(BaseObject);
                //建立用电关系
                IElecUser<ICharacter> elecUser = attachedItem.AttachedColl.GetComponent<IElecUser<ICharacter>>();
                if (elecUser != null)
                {
                    BaseObject.SetElecUser(elecUser);
                    BaseObject.ElecUser.SetElecPower(BaseObject);
                }
                //返回附着状态
                return new Attached(BaseObject);
            }
        }
        return null;
    }
    public void Leave()
    {
        Debug.Log("Leave UnAttached");
    }
    private IAttachedBy<ICharacter> FindMinAttachedItem(Vector2 pointA, Vector2 pointB)
    {
        Collider2D[] colliders = new Collider2D[0];
        colliders = Physics2D.OverlapAreaAll(pointA, pointB);
        float minDistance = Mathf.Infinity;
        float distance;
        IAttachedBy<ICharacter> nearest = null;
        foreach (Collider2D collider in colliders)
        {
            IAttachedBy<ICharacter> attachedItem = collider.GetComponent<IAttachedBy<ICharacter>>();
            if (attachedItem != null)
            {
                distance = Vector2.Distance(BaseObject.Collider2D.bounds.center, collider.bounds.center);
                if (distance < minDistance)
                {
                    nearest = attachedItem;
                    minDistance = distance;
                }
            }
        }
        return nearest;
    }
    private IAttachedBy<ICharacter> FindAttachableItem()
    {
        Debug.Log("FindAttachableItem");
        CalCulateCheckArea();
        IAttachedBy<ICharacter> nearest = null;
        if (horizontal >= 0)
        {
            Debug.Log("checkMaxRU:" + checkMaxRU + ", checkMinRD:" + checkMinRD);
            nearest = FindMinAttachedItem(checkMaxRU, checkMinRD); // 1 5
            if (nearest != null)
                return nearest;
        }
        if (horizontal <= 0)
        {
            Debug.Log("checkMaxLU:" + checkMaxLU + ", checkMinLD:" + checkMinLD);
            nearest = FindMinAttachedItem(checkMaxLU, checkMinLD); // 2 4
            if (nearest != null)
                return nearest;
        }
        if (vertical >= 0)
        {
            Debug.Log("checkMaxRU:" + checkMaxRU + ", checkMinLU:" + checkMinLU);
            nearest = FindMinAttachedItem(checkMaxRU, checkMinLU); // 1 3
            if (nearest != null)
                return nearest;
        }
        if (vertical <= 0)
        {
            Debug.Log("checkMaxRD:" + checkMaxRD + ", checkMinLD:" + checkMinLD);
            nearest = FindMinAttachedItem(checkMaxRD, checkMinLD);// 6 4
            if (nearest != null)
                return nearest;
        }
        return null;
    }
    private void CalCulateCheckArea()
    {
        Bounds collBounds = BaseObject.Collider2D.bounds;
        checkMaxRU = new Vector2(collBounds.max.x + checkOffset, collBounds.max.y + checkOffset); // 1
        checkMaxLU = new Vector2(collBounds.min.x, collBounds.max.y + checkOffset); // 2
        checkMinLU = new Vector2(collBounds.min.x - checkOffset, collBounds.max.y); // 3

        checkMinLD = new Vector2(collBounds.min.x - checkOffset, collBounds.min.y - checkOffset); // 4
        checkMinRD = new Vector2(collBounds.max.x, collBounds.min.y - checkOffset); // 5
        checkMaxRD = new Vector2(collBounds.max.x + checkOffset, collBounds.min.y); // 6
    }
}
class Attached : IState<ICharacter>
{
    private float originGravityScale;
    public float countFrames = 15.0f;
    public float checkRediums;
    public float leaveOffset = 0.1f;
    private Vector3 leaveCenter;
    private Bounds preBounds;

    private float horizontal;
    private float vertical;

    private ICharacter character;
    public ICharacter BaseObject
    {
        get => character;
        set => character = value;
    }
    public Attached(ICharacter character)
    {
        BaseObject = character;
    }
    public void Enter()
    {
        Debug.Log("Enter Attached");
        //记录重力
        originGravityScale = character.Rigidbody2D.gravityScale;
        //记录初始碰撞体积
        preBounds = character.Collider2D.bounds;
        //重力置为0
        character.Rigidbody2D.gravityScale = 0.0f;
        //速度置为0
        character.Rigidbody2D.velocity = Vector2.zero;
        //取消碰撞体积
        character.Collider2D.enabled = false;
        //播放进入动画
        character.StartCoroutine(EnterAnimation());
    }
    public IState<ICharacter> Handler()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (UnAttachable())
            {
                //设置用电器关闭
                BaseObject.PowerOff();
                //取消用电关系
                BaseObject.ElecUser.SetElecPower(null);
                BaseObject.SetElecUser(null);
                //取消连接
                BaseObject.AttachedItem.SetAttachable(null);
                BaseObject.SetAttachedItem(null);
                //返回冲刺状态
                return new Dash(BaseObject, horizontal, vertical);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //设置用电器开启
            BaseObject.PowerOn();
        }
        return null;
    }
    public void Leave()
    {
        Debug.Log("Leave Attached");
        //播放退出动画
        character.StartCoroutine(LeaveAnimation());
        //恢复重力
        character.Rigidbody2D.gravityScale = originGravityScale;
        //恢复碰撞体积
        character.Collider2D.enabled = true;
    }
    private bool UnAttachable()
    {
        Bounds bounds = BaseObject.AttachedItem.AttachedColl.bounds;
        float xOffset = preBounds.extents.x + BaseObject.AttachedItem.AttachedColl.bounds.extents.x + leaveOffset;
        float yOffset = preBounds.extents.y + BaseObject.AttachedItem.AttachedColl.bounds.extents.y + leaveOffset;
        float xDir = horizontal;
        float yDir = vertical;
        xDir = xDir > 0 ? 1.0f : xDir;
        xDir = xDir < 0 ? -1.0f : xDir;
        yDir = yDir > 0 ? 1.0f : yDir;
        yDir = yDir < 0 ? -1.0f : yDir;
        leaveCenter = new Vector3(bounds.center.x + xOffset * xDir, bounds.center.y + yOffset * yDir, bounds.center.z);
        Vector2 checkMax = new Vector2(leaveCenter.x + preBounds.extents.x, leaveCenter.y + preBounds.extents.y);
        Vector2 checkMin = new Vector2(leaveCenter.x - preBounds.extents.x, leaveCenter.y - preBounds.extents.y);
        Debug.Log("xDir:" + xDir + "yDir:" + yDir);
        Debug.Log("xOffset:" + xOffset + "yOffset:" + yOffset);
        Debug.Log("LeaveCenter:" + leaveCenter);
        Debug.Log("checkMax:" + checkMax + "checkMin:" + checkMin);
        Collider2D[] colls = new Collider2D[0];
        //方形检测
        // colls = Physics2D.OverlapAreaAll(checkMax, checkMin);
        //圆形检测
        checkRediums = Mathf.Max(preBounds.extents.x, preBounds.extents.y);
        colls = Physics2D.OverlapCircleAll((checkMax + checkMin) / 2, checkRediums);
        //隔板不能穿过
        foreach (Collider2D collider in colls)
        {
            ISeptum septum = collider.GetComponent<ISeptum>();
            if (septum != null)
            {
                return false;
            }
        }
        //地面不能穿过
        foreach (Collider2D collider in colls)
        {
            IGround ground = collider.GetComponent<IGround>();
            if (ground != null)
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator EnterAnimation()
    {
        Debug.Log("EnterAnimation");
        float delta = 1.0f / countFrames;
        Vector3 deltaScale = new Vector3(delta, delta, delta);
        Vector3 deltaDistance = (BaseObject.AttachedItem.AttachedColl.bounds.center - character.Collider2D.bounds.center) / countFrames;
        for (int i = 0; i < countFrames; i++)
        {
            character.Transform.position += deltaDistance;
            character.Transform.localScale -= deltaScale;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator LeaveAnimation()
    {
        Debug.Log("LeaveAnimation");
        float delta = 1.0f / countFrames;
        Vector3 deltaScale = new Vector3(delta, delta, delta);
        character.Transform.position = leaveCenter;
        for (int i = 0; i < countFrames; i++)
        {
            character.Transform.localScale += deltaScale;
            yield return new WaitForFixedUpdate();
        }
    }
}
class Stop : UnAttached, IState<ICharacter>
{
    public IMoveCommand stopMoveCommand;
    float groundCheckRadius = 0.1f;
    public Stop(ICharacter character) : base(character)
    {
    }
    public new void Enter()
    {
        base.Enter();
        Debug.Log("Enter Stop");
        stopMoveCommand = new MoveCommand(BaseObject, 0.0f, 0.0f, 0.0f, 0.0f);
        stopMoveCommand.Handle();
    }
    public new IState<ICharacter> Handler()
    {
        IState<ICharacter> baseHandler = base.Handler();
        if (baseHandler != null)
        {
            return baseHandler;
        }
        Debug.Log("Handler Stop");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //控制移动
        if (horizontal != 0)
        {
            ICommand moveCommand = new MoveCommand(BaseObject, horizontal, BaseObject.CharacterImp.moveXSpeed, 1, BaseObject.Rigidbody2D.velocity.y);
            moveCommand.Handle();
        }
        if (Input.GetKeyDown(KeyCode.J) && Dashable())
        {
            horizontal = horizontal > 0.0f ? 1.0f : horizontal;
            horizontal = horizontal < 0.0f ? -1.0f : horizontal;
            vertical = vertical > 0.0f ? 1.0f : vertical;
            vertical = vertical < 0.0f ? -1.0f : vertical;
            return new Dash(BaseObject, horizontal, vertical);
        }
        return null;
    }
    public new void Leave()
    {
        Debug.Log("Leave Stop");
        base.Leave();
    }
    private bool Dashable()
    {
        Debug.Log("Dashable Stop");
        Collider2D[] colls = new Collider2D[0];
        Vector2 checkMin = new Vector2(BaseObject.Collider2D.bounds.center.x, BaseObject.Collider2D.bounds.center.y - BaseObject.Collider2D.bounds.extents.y);
        Debug.Log("checkMin:" + checkMin + " groundCheckRadius:" + groundCheckRadius);
        colls = Physics2D.OverlapCircleAll(checkMin, groundCheckRadius);
        foreach (Collider2D coll in colls)
        {
            Debug.Log("coll" + coll.bounds.center);
            IGround ground = coll.GetComponent<IGround>();
            ISlider slider = coll.GetComponent<ISlider>();
            if (ground != null || slider != null)
            {
                return true;
            }
        }
        return false;
    }
}
class Dash : UnAttached, IState<ICharacter>
{
    public float horizontal;
    public float vertical;
    private float preGravityScale;
    private IMoveCommand dashCommand;
    private float dashStartTime;
    public Dash(ICharacter character, float horizontal, float vertical) : base(character)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }
    public new void Enter()
    {
        base.Enter();
        Debug.Log("Enter Dash");
        //取消重力
        preGravityScale = BaseObject.Rigidbody2D.gravityScale;
        BaseObject.Rigidbody2D.gravityScale = 0;
        //冲刺初始化
        dashStartTime = Time.time;
        dashCommand = new MoveCommand(BaseObject, horizontal, BaseObject.CharacterImp.dashXSpeed, vertical, BaseObject.CharacterImp.dashYSpeed);
        //冲刺移动
        dashCommand.Handle();
        //冲刺动画
        BaseObject.StartCoroutine(DashShowder());
    }
    public new IState<ICharacter> Handler()
    {
        IState<ICharacter> baseHandler = base.Handler();
        if (baseHandler != null)
        {
            return baseHandler;
        }
        Debug.Log("Handler Dash");
        if (Time.time < dashStartTime + BaseObject.CharacterImp.dashTime)
        {
            return null;
        }
        return new Stop(BaseObject);
    }
    public new void Leave()
    {
        Debug.Log("Leave Dash");
        //恢复重力
        BaseObject.Rigidbody2D.gravityScale = preGravityScale;
        base.Leave();
    }
    private IEnumerator DashShowder()
    {
        while (Time.time <= dashStartTime + BaseObject.CharacterImp.dashTime)
        {
            ShadowPool.instance.GetFromPool();
            yield return new WaitForFixedUpdate();
        }
    }
}