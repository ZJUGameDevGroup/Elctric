using UnityEngine;
public enum EleFBodyType
{
    //点电荷
    POINT,
    //带电球体
    CIRCLE,
    //带电导线
    LINE,
    //带电平面
    PLATFORM,
    //带电环
    RING
}
public interface IEleFBody
{
    //导体类型
    EleFBodyType Type { get; }
    //是否可移动
    float Mobility { get; }
    //带电量
    float Electric { get; set; }
    //场区域
    IElecField ElecField { get; set; }
    //产生场强
    float GeneEleField(Vector3 postion);
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EleFBodyImp", order = 1)]
public class EleFBodyImp : ScriptableObject
{
    [Header("导体属性")]
    public float mobility;
    public float electric;
    public EleFBodyType type;
}
public class EleFBody : IEleFBody
{
    //固定属性
    EleFBodyImp eleFBodyImp;
    //电场区域
    IElecField elecField;
    //导体类型
    public EleFBodyType Type
    {
        get => eleFBodyImp.type;
    }
    //是否可移动
    public float Mobility
    {
        get => eleFBodyImp.mobility;
    }
    //带电量
    public float Electric
    {
        get => eleFBodyImp.electric;
        set => eleFBodyImp.electric = value;
    }
    //电场区域
    public IElecField ElecField
    {
        get => elecField;
        set => elecField = value;
    }
    //产生场强
    public float GeneEleField(Vector3 postion)
    {
        return 0;
    }
}
public class PointEleFBody : EleFBody, IEleFBody
{
    //产生场强
    public new float GeneEleField(Vector3 postion)
    {
        if (Electric == 0)
        {
            return 0;
        }
        Vector3 center = ElecField.Area.bounds.center;
        float distance = Vector3.Distance(center, postion);
        return Electric / Mathf.Pow(distance, 2);
    }
}
public class CircleEleFBody : EleFBody, IEleFBody
{
    float radius;
    public float Radius
    {
        get => radius;
        set => radius = value;
    }
    //产生场强
    public new float GeneEleField(Vector3 postion)
    {
        if (Electric == 0)
        {
            return 0;
        }
        Vector3 center = ElecField.Area.bounds.center;
        float distance = Vector3.Distance(center, postion);
        if (distance <= radius)
        {
            return 0;
        }
        return Electric / Mathf.Pow(distance, 2);
    }
}
public class LineEleFBody : EleFBody, IEleFBody
{
    //产生场强
    public new float GeneEleField(Vector3 postion)
    {
        if (Electric == 0)
        {
            return 0;
        }
        Vector3 center = ElecField.Area.bounds.center;
        float distance = Vector3.Distance(center, postion);

        return Electric / Mathf.Pow(distance, 2);
    }
}
public class PlatEleFBody : EleFBody, IEleFBody
{
    //产生场强
    public new float GeneEleField(Vector3 postion)
    {
        if (Electric == 0)
        {
            return 0;
        }
        Vector3 center = ElecField.Area.bounds.center;
        float distance = Vector3.Distance(center, postion);

        return Electric / Mathf.Pow(distance, 2);
    }
}
public class RingEleFBody : EleFBody, IEleFBody
{
    //产生场强
    public new float GeneEleField(Vector3 postion)
    {
        if (Electric == 0)
        {
            return 0;
        }
        Vector3 center = ElecField.Area.bounds.center;
        float distance = Vector3.Distance(center, postion);

        return Electric / Mathf.Pow(distance, 2);
    }
}