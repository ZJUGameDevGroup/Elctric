using UnityEngine;
public interface IPlayer : ICharacter
{
    IContext<ICharacter> Context { get; set; }
}
class Player : Character, IPlayer
{
    //状态机
    private IContext<ICharacter> context;
    public IContext<ICharacter> Context
    {
        get => context;
        set => context = value;
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
    }
    private void Start()
    {
        context = new Context<ICharacter>(new Stop(this));
        context.Enter();
    }
    private void Update()
    {
        context.Handler();
    }

}