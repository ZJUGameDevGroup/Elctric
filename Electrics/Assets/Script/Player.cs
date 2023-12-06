using UnityEngine;
public interface IPlayer : ICharacter
{
    IContext<ICharacter> Context { get; set; }
}
class Player : Character, IPlayer
{
    //״̬��
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
        //ȡ������
        rb.drag = 0;
        //������ʼ��
        rb.gravityScale = 8;
        //ȡ����ת
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