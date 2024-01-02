using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilitySystem : MonoBehaviour
{
    //��������
    private float durabilityValue;
    public float DurabilityValue
    {
        get
        {
            return durabilityValue;
        }
        set
        {
            durabilityValue = Mathf.Clamp(value, 0, 10f);
        }
    }

    public void DecreaseDurabilityValue(float decreaseValue)
    {
        DurabilityValue -= decreaseValue;
    }
}
