public interface IState<T>
{
    T BaseObject { get; set; }
    void Enter();
    IState<T> Handler();
    void Leave();
}
public interface IContext<T>
{
    IState<T> State
    {
        get; set;
    }
    T BaseObject { get; set; }
    void Enter();
    void Handler();
    void Leave();
}
class Context<T> : IContext<T>
{
    public IState<T> state;
    private T baseObject;
    public T BaseObject
    {
        get => baseObject;
        set => baseObject = value;
    }
    public IState<T> State
    {
        get => state;
        set => state = value;
    }
    public Context(IState<T> state)
    {
        this.state = state;
    }
    public void Enter()
    {
        if (state != null)
        {
            state.Enter();
        }
    }
    public void Handler()
    {
        if (state != null)
        {
            IState<T> _state = state.Handler();
            if (_state != null)
            {
                state.Leave();
                state = _state;
                state.Enter();
            }
        }
    }
    public void Leave()
    {
        if (state != null)
        {
            state.Leave();
        }
    }
}