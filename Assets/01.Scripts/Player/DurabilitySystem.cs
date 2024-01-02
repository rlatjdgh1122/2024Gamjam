using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilitySystem : MonoBehaviour
{
    //내구도임
    private float durabilityValue;

    [SerializeField]
    private float maxDurabilityValue;

    public float DurabilityValue
    {
        get
        {
            return durabilityValue;
        }
        set
        {
            durabilityValue = Mathf.Clamp(value, 0, maxDurabilityValue);
        }
    }

    public void DecreaseDurabilityValue(float decreaseValue)
    {
        DurabilityValue -= decreaseValue;
    }
}
