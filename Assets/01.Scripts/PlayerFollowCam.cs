using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    public static PlayerFollowCam Instance;

    private CinemachineVirtualCamera _followCam;
    private CinemachineBasicMultiChannelPerlin m_channelsPerlin;

    [SerializeField]
    private float followCamFOVMultipleValue = 2;

    private Transform _speedLineEffect;

    [SerializeField]
    private float totalMultipleValue;

    private void Awake()
    {
        Instance = this;

        _followCam = GetComponent<CinemachineVirtualCamera>();
        //_speedLineEffect = PlayerManager.Instance.Player.transform.Find("SpeedLineEffect");
        m_channelsPerlin = _followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void LateUpdate()
    {
        totalMultipleValue = PlayerManager.Instance.GetBuringSystem.BurningValue * followCamFOVMultipleValue;
        _followCam.m_Lens.FieldOfView = Mathf.Lerp(_followCam.m_Lens.FieldOfView, 60 + totalMultipleValue, Time.deltaTime);

        //_speedLineEffect.DOMoveZ(PlayerManager.Instance.GetBuringSystem.MaxBurningValue);
    }

    public void ShakeCam(float delay, float value)
    {
        StartCoroutine(CamShake(delay, value));
    }

    private IEnumerator CamShake(float delay, float value)
    {
        m_channelsPerlin.m_AmplitudeGain = value;
        yield return new WaitForSeconds(delay);
        m_channelsPerlin.m_AmplitudeGain = 0;
    }
}
