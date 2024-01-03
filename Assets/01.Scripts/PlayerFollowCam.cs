using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    private CinemachineVirtualCamera _followCam;

    [SerializeField]
    private float followCamFOVMultipleValue = 2;


    [SerializeField]
    private float totalMultipleValue;

    private void Awake()
    {
        _followCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        totalMultipleValue = PlayerManager.Instance.GetBuringSystem.BurningValue * followCamFOVMultipleValue;
        _followCam.m_Lens.FieldOfView = Mathf.Lerp(_followCam.m_Lens.FieldOfView, 60 + totalMultipleValue, Time.deltaTime);
    }
}
