using UnityEngine;
public enum EleFBodyType
{
    //����
    POINT,
    //��������
    CIRCLE,
    //���絼��
    LINE,
    //����ƽ��
    PLATFORM,
    //���绷
    RING
}
public interface IEleFBody
{
    //��������
    EleFBodyType Type { get; }
    //�Ƿ���ƶ�
    float Mobility { get; }
    //������
    float Electric { get; set; }
    //������
    IElecField ElecField { get; set; }
    //������ǿ
    float GeneEleField(Vector3 postion);
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EleFBodyImp", order = 1)]
public class EleFBodyImp : ScriptableObject
{
    [Header("��������")]
    public float mobility;
    public float electric;
    public EleFBodyType type;
}
public class EleFBody : IEleFBody
{
    //�̶�����
    EleFBodyImp eleFBodyImp;
    //�糡����
    IElecField elecField;
    //��������
    public EleFBodyType Type
    {
        get => eleFBodyImp.type;
    }
    //�Ƿ���ƶ�
    public float Mobility
    {
        get => eleFBodyImp.mobility;
    }
    //������
    public float Electric
    {
        get => eleFBodyImp.electric;
        set => eleFBodyImp.electric = value;
    }
    //�糡����
    public IElecField ElecField
    {
        get => elecField;
        set => elecField = value;
    }
    //������ǿ
    public float GeneEleField(Vector3 postion)
    {
        return 0;
    }
}
public class PointEleFBody : EleFBody, IEleFBody
{
    //������ǿ
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
    //������ǿ
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
    //������ǿ
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
    //������ǿ
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
    //������ǿ
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