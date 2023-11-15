using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecItem : MonoBehaviour
{
    public bool isEntered = false;

    public void Entered()
    {
        isEntered  = !isEntered;
    }
}
