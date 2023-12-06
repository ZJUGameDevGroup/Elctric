using UnityEngine;
public interface ISeptum : ICollisible
{
}
public class Septum : Collisible, ISeptum
{
    public void Start()
    {
        Collider2D.isTrigger = true;
    }
}