using UnityEngine;
public interface IBackWall : ICollisible
{
}
public class BackWall : Collisible, IBackWall
{
    public void Start()
    {
        Collider2D.isTrigger = true;
    }
}