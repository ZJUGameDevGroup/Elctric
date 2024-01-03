using UnityEngine;
public interface ICollisible
{
    Collider2D Collider2D
    {
        get; set;
    }
}
public class Collisible : MonoBehaviour, ICollisible
{
    public Collider2D coll2D;
    public Collider2D Collider2D
    {
        get => coll2D;
        set => coll2D = value;
    }
    public void Awake()
    {
        Collider2D = gameObject.GetComponent<Collider2D>();
        if (Collider2D == null)
        {
            Collider2D = gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
