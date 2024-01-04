using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilitySystem : MonoBehaviour
{
    //내구도임
    private float durabilityValue;

    [SerializeField]
    private float firstSettingValue;
    [SerializeField]
    public float MaxValue;

    public float DurabilityValue
    {
        get
        {
            return durabilityValue;
        }
        set
        {
            durabilityValue = Mathf.Clamp(value, 0, MaxValue);
        }
    }

    private void Start()
    {
        durabilityValue = firstSettingValue;
    }

    public void ChangeValue(float decreaseValue)
    {
        DOTween.To(() => DurabilityValue, x => DurabilityValue = x, DurabilityValue + decreaseValue, 1.0f).SetEase(Ease.OutQuart);
    }

    public void MinusValue(float value)
    {
        DurabilityValue -= value;
    }
}
