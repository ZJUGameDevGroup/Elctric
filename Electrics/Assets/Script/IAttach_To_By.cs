using UnityEngine;
public interface IAttachTo<T>
{
    T AttachedItem { get; set; }
    Collider2D AttachableColl { get; set; }
    void SetAttachedItem(T attachedItem);
}
public interface IAttachedBy<T>
{
    T AttachableItem { get; set; }
    Collider2D AttachedColl { get; set; }
    void SetAttachable(T attachableItem);
}
