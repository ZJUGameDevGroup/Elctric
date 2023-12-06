using UnityEngine;
public interface IMove
{
    void Move(float horizontal, float horizontalSpeed, float vertical, float verticalSpeed);
}
public interface IForce
{
    void Force(Vector2 right, float rightForce, Vector2 up, float upForce);
}
public interface IActor
{
    void Act();
}
public interface IMoveActor : IActor
{
    IMove MoveItem { get; set; }
}
class MoveActor : IMoveActor
{
    private float horizontal;
    private float vertical;
    private float horizontalSpeed;
    private float verticalSpeed;
    private IMove moveItem;
    public IMove MoveItem
    {
        get => moveItem;
        set => moveItem = value;
    }
    public MoveActor(IMove moveItem, float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        MoveItem = moveItem;
        this.horizontal = horizontal;
        this.vertical = vertical;
        this.horizontalSpeed = horizontalSpeed;
        this.verticalSpeed = verticalSpeed;
    }
    public void Act()
    {
        moveItem.Move(horizontal, horizontalSpeed, vertical, verticalSpeed);
    }
}
public interface IForceActor : IActor
{
    IForce ForceItem { get; set; }
}
class ForceActor : IForceActor
{
    private float rightForce;
    private float upForce;
    private Vector2 right;
    private Vector2 up;
    private IForce forceItem;
    public IForce ForceItem
    {
        get => forceItem;
        set => forceItem = value;
    }
    public ForceActor(IForce forceItem, Vector2 right, float rightForce, Vector2 up, float upForce)
    {
        ForceItem = forceItem;
        this.up = up;
        this.right = right;
        this.upForce = upForce;
        this.rightForce = rightForce;
    }
    public void Act()
    {
        forceItem.Force(right, rightForce, up, upForce);
    }
}

public interface ICommand
{
    void Handle();
}
public interface IMoveCommand : ICommand
{
    IMoveActor MoveActor { get; set; }
}
class MoveCommand : IMoveCommand
{
    IMoveActor moveActor;
    public IMoveActor MoveActor
    {
        get => moveActor;
        set => moveActor = value;
    }
    public MoveCommand(IMove moveItem, float horizontal, float horizontalSpeed, float vertical, float verticalSpeed)
    {
        moveActor = new MoveActor(moveItem, horizontal, horizontalSpeed, vertical, verticalSpeed);
    }
    public void Handle()
    {
        moveActor.Act();
    }
}
public interface IForceCommand : ICommand
{
    IForceActor ForceActor { get; set; }
}
class ForceCommand : IForceCommand
{
    IForceActor forceActor;
    public IForceActor ForceActor
    {
        get => forceActor;
        set => forceActor = value;
    }
    public ForceCommand(IForce forceItem, Vector2 right, float rightForce, Vector2 up, float upForce)
    {
        forceActor = new ForceActor(forceItem, right, rightForce, up, upForce);
    }
    public void Handle()
    {
        forceActor.Act();
    }
}