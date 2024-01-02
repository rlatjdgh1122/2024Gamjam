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
    private float maxValue;

    public float DurabilityValue
    {
        get
        {
            return durabilityValue;
        }
        set
        {
            durabilityValue = Mathf.Clamp(value, 0, maxValue);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeValue(-0.1f);
        }
    }
}
