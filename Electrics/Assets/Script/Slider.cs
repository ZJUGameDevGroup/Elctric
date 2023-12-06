using UnityEngine;
public enum Direction
{
    UP, DOWN, LEFT, RIGHT
}
public interface ISlider : IElecUser<ICharacter>, IAttachedBy<ICharacter>, IMove
{
    Direction Direction { get; set; }
    BackWall BackWall { get; set; }
    Collider2D Collider2D { get; set; }
    Rigidbody2D Rigidbody2D { get; set; }
    IContext<ISlider> Context { get; set; }
    float MoveSpeed { get; set; }
    void RevertToMoveable();
    void RevertToDisMoveable();
}
public class Slider : MonoBehaviour, ISlider
{
    public Direction direction;
    public BackWall backWall;
    public Collider2D coll;
    public Rigidbody2D rb;
    private IContext<ISlider> context;
    private float slideTime = 0.25f;
    public float moveSpeed;
    private ICharacter power;
    public Direction Direction
    {
        get => direction;
        set => direction = value;
    }
    public BackWall BackWall
    {
        get => backWall;
        set => backWall = value;
    }
    public Collider2D Collider2D
    {
        get => coll;
        set => coll = value;
    }
    public Rigidbody2D Rigidbody2D
    {
        get => rb;
        set => rb = value;
    }
    public IContext<ISlider> Context
    {
        get => context;
        set => context = value;
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public ICharacter ElecPower
    {
        get => power;
        set => power = value;
    }
    public ICharacter AttachableItem
    {
        get => power;
        set => power = value;
    }
    public Collider2D AttachedColl
    {
        get => coll;
        set => coll = value;
    }
    public void Open()
    {
        Debug.Log("Slider Open");
        context.Leave();
        context.State = new OpenState(this);
        context.Enter();
    }
    public void Close()
    {
        Debug.Log("Slider Close");
        context.Leave();
        context.State = new SlideState(this, slideTime);
        context.Enter();
    }
    public void Move(float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        Debug.Log("Slider Move");
        rb.velocity = new Vector2(horizontal * horizontalSpeed, vertical * verticalSpeed);
    }
    public void SetAttachable(ICharacter attachableItem)
    {
        Debug.Log("Slider SetAttachable");
        AttachableItem = attachableItem;
    }
    public void SetElecPower(ICharacter elecPower)
    {
        Debug.Log("Slider SetAttachable");
        ElecPower = elecPower;
    }
    public void RevertToMoveable()
    {
        if (Rigidbody2D == null) return;
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody2D.gravityScale = 0;
        Rigidbody2D.drag = 0;

    }
    public void RevertToDisMoveable()
    {
        if (Rigidbody2D == null) return;
        Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<Collider2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        if (coll == null)
        {
            coll = gameObject.AddComponent<BoxCollider2D>();
        }
        //取消阻力
        rb.drag = 0;
        //重力初始化
        rb.gravityScale = 8;
        //取消旋转
        rb.freezeRotation = true;
        //设置类型
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    private void Start()
    {
        // context = new Context<ISlider>(new CloseState(this));
        context = new Context<ISlider>(null);
        context.Enter();
    }
    private void Update()
    {
        context.Handler();
    }
}

public class OpenState : IState<ISlider>
{
    public float checkRadius = 0.1f;
    private ISlider slider;
    private IMoveCommand moveCommand;
    public ISlider BaseObject
    {
        get => slider;
        set => slider = value;
    }
    public IMoveCommand MoveCommand
    {
        get => moveCommand;
        set => moveCommand = value;
    }
    public OpenState(ISlider slider)
    {
        BaseObject = slider;
    }
    private void StartMove()
    {
        switch (BaseObject.Direction)
        {
            case Direction.LEFT:
                MoveCommand = new MoveCommand(BaseObject, -1, BaseObject.MoveSpeed, 0, 0);
                break;
            case Direction.RIGHT:
                MoveCommand = new MoveCommand(BaseObject, 1, BaseObject.MoveSpeed, 0, 0);
                break;
            case Direction.UP:
                MoveCommand = new MoveCommand(BaseObject, 0, 0, 1, BaseObject.MoveSpeed);
                break;
            case Direction.DOWN:
                MoveCommand = new MoveCommand(BaseObject, 0, 0, -1, BaseObject.MoveSpeed);
                break;

        }
        MoveCommand.Handle();
    }
    public void Enter()
    {
        Debug.Log("OpenState Enter");
        //转换成可运动状态
        BaseObject.RevertToMoveable();
        StartMove();
    }
    public IState<ISlider> Handler()
    {
        Debug.Log("OpenState Handler");
        if (Collision())
        {
            Debug.Log("OpenState Handler Collision");
            return new CloseState(BaseObject);
        }
        float wallMinX = BaseObject.BackWall.Collider2D.bounds.min.x;
        float wallMinY = BaseObject.BackWall.Collider2D.bounds.min.y;
        float wallMaxX = BaseObject.BackWall.Collider2D.bounds.max.x;
        float wallMaxY = BaseObject.BackWall.Collider2D.bounds.max.y;

        float objMinX = BaseObject.Collider2D.bounds.min.x;
        float objMinY = BaseObject.Collider2D.bounds.min.y;
        float objMaxX = BaseObject.Collider2D.bounds.max.x;
        float objMaxY = BaseObject.Collider2D.bounds.max.y;

        bool atEnd = false;
        switch (BaseObject.Direction)
        {
            case Direction.LEFT:
                if (Mathf.Abs(objMinX - wallMinX) < checkRadius)
                {
                    Debug.Log("wallMinX:" + wallMinX + " + objMinX : " + objMinX);
                    BaseObject.Direction = Direction.RIGHT;
                    atEnd = true;
                }
                break;
            case Direction.RIGHT:
                if (Mathf.Abs(objMaxX - wallMaxX) < checkRadius)
                {
                    Debug.Log("wallMaxX:" + wallMaxX + " + objMaxX : " + objMaxX);
                    BaseObject.Direction = Direction.LEFT;
                    atEnd = true;
                }
                break;
            case Direction.UP:
                if (Mathf.Abs(objMaxY - wallMaxY) < checkRadius)
                {
                    Debug.Log("wallMaxY:" + wallMaxY + " + objMaxY : " + objMaxY);
                    BaseObject.Direction = Direction.DOWN;
                    atEnd = true;
                }
                break;
            case Direction.DOWN:
                if (Mathf.Abs(objMinY - wallMinY) < checkRadius)
                {
                    Debug.Log("wallMinY:" + wallMinY + " + objMinY : " + objMinY);
                    BaseObject.Direction = Direction.UP;
                    atEnd = true;
                }
                break;

        }
        if (atEnd)
        {
            StartMove();
        }
        return null;
    }
    public void Leave()
    {
        Debug.Log("OpenState Leave");
        BaseObject.RevertToDisMoveable();
    }
    public bool Collision()
    {
        Debug.Log("OpenState Collision");
        Collider2D[] colliders = new Collider2D[0];
        Direction dir = BaseObject.Direction;
        Vector3 maxPoint = BaseObject.Collider2D.bounds.max;
        Vector3 minPoint = BaseObject.Collider2D.bounds.min;
        float xRadius = BaseObject.Collider2D.bounds.extents.x;
        float yRadius = BaseObject.Collider2D.bounds.extents.y;
        Vector2 extent;
        Vector2 checkPoint;
        switch (BaseObject.Direction)
        {
            case Direction.UP:
                checkPoint = new Vector2(minPoint.x, maxPoint.y + checkRadius);
                extent = new Vector2(xRadius, checkRadius);
                colliders = Physics2D.OverlapAreaAll(checkPoint, checkPoint + 2 * extent);
                break;
            case Direction.DOWN:
                checkPoint = new Vector2(maxPoint.x, minPoint.y - checkRadius);
                extent = new Vector2(xRadius, checkRadius);
                colliders = Physics2D.OverlapAreaAll(checkPoint, checkPoint - 2 * extent);
                break;
            case Direction.LEFT:
                checkPoint = new Vector2(minPoint.x - checkRadius, maxPoint.y);
                extent = new Vector2(checkRadius, yRadius);
                colliders = Physics2D.OverlapAreaAll(checkPoint, checkPoint - 2 * extent);
                break;
            case Direction.RIGHT:
                checkPoint = new Vector2(maxPoint.x + checkRadius, minPoint.y);
                extent = new Vector2(checkRadius, yRadius);
                colliders = Physics2D.OverlapAreaAll(checkPoint, checkPoint + 2 * extent);
                break;
        }
        if (colliders.Length == 0)
        {
            return false;
        }
        foreach (Collider2D collider in colliders)
        {
            ISlider collSlider = collider.GetComponent<ISlider>();
            if (collSlider != null)
            {
                return true;
            }
        }
        return false;
    }
}
public class SlideState : OpenState, IState<ISlider>
{
    public float slideTimeLeft;
    public float startTime;
    public SlideState(ISlider slider, float slideTimeLeft) : base(slider)
    {
        this.slideTimeLeft = slideTimeLeft;
    }
    public new void Enter()
    {
        base.Enter();
        Debug.Log("SlideState Enter");
        startTime = Time.time;
    }
    public new IState<ISlider> Handler()
    {
        IState<ISlider> _state = base.Handler();
        if (_state != null )
        {
            return _state;
        }
        Debug.Log("CloseState Handler");
        if (slideTimeLeft + startTime < Time.time)
        {
            return new CloseState(BaseObject);
        }
        return null;
    }
    public new void Leave()
    {
        Debug.Log("CloseState Leave");
        base.Leave();
    }
}
public class CloseState : IState<ISlider>
{
    private ISlider slider;
    private IMoveCommand moveCommand;
    public ISlider BaseObject
    {
        get => slider;
        set => slider = value;
    }
    public IMoveCommand MoveCommand
    {
        get => moveCommand;
        set => moveCommand = value;
    }
    public CloseState(ISlider slider)
    {
        BaseObject = slider;
    }
    public void Enter()
    {
        Debug.Log("CloseState Enter");
        MoveCommand = new MoveCommand(BaseObject, 0, 0, 0, 0);
        moveCommand.Handle();
    }
    public IState<ISlider> Handler()
    {
        Debug.Log("CloseState Handler");
        return null;
    }
    public void Leave()
    {
        Debug.Log("CloseState Leave");
    }
}
