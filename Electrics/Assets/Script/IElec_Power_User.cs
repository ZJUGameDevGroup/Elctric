public interface IElecPower<T>
{
    T ElecUser { get; set; }
    void SetElecUser(T elecUser);
    void PowerOn();
    void PowerOff();
}
public interface IElecUser<T>
{
    T ElecPower { get; set; }
    void SetElecPower(T elecPower);
    void Open();
    void Close();
}
