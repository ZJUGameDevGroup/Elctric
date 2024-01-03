using UnityEngine;

public interface IElecField
{
    Collider2D Area
    {
        get; set;
    }
}
public class ElecField : IElecField
{
    Collider2D area;
    public void Start()
    {
        if (area == null)
        {

        }
        area.isTrigger = true;
    }
    public Collider2D Area
    {
        get => area;
        set
        {
            area = value;
            area.isTrigger = true;
        }
    }
}