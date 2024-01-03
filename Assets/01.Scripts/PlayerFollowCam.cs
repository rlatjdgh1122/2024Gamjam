using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    private CinemachineVirtualCamera _followCam;

    [SerializeField]
    private float followCamFOVMultipleValue = 2;

    private Transform _speedLineEffect;

    [SerializeField]
    private float totalMultipleValue;

    private void Awake()
    {
        _followCam = GetComponent<CinemachineVirtualCamera>();
        //_speedLineEffect = PlayerManager.Instance.Player.transform.Find("SpeedLineEffect");
    }

    private void LateUpdate()
    {
        totalMultipleValue = PlayerManager.Instance.GetBuringSystem.BurningValue * followCamFOVMultipleValue;
        _followCam.m_Lens.FieldOfView = Mathf.Lerp(_followCam.m_Lens.FieldOfView, 60 + totalMultipleValue, Time.deltaTime);

        //_speedLineEffect.DOMoveZ(PlayerManager.Instance.GetBuringSystem.MaxBurningValue);
    }
}
