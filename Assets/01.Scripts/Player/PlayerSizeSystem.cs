using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSizeSystem : MonoBehaviour
{
    private DurabilitySystem _durability;
    public Transform sizeApplyObj;
    private void Awake()
    {
        _durability = GetComponent<DurabilitySystem>();
    }

    private void Update()
    {
        sizeApplyObj.localScale = new Vector3(_durability.DurabilityValue, _durability.DurabilityValue, _durability.DurabilityValue);
    }
}
