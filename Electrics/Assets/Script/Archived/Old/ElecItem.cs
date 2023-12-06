using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecItem : MonoBehaviour
{
    public bool charged = false;
    public enum Direction {UD, LR};
    public Direction moveDirection;
    public Transform beginTransform;
    public Transform endTransform;
    public void ChangeChaged()
    {
        charged  = !charged;
    }
}
