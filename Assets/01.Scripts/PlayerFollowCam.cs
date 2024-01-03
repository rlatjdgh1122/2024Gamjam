using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    [Header("게임 설정")]
    public float startTargetValue = 50f;

    [Header("카메라 세팅 설정")]
    public float minLensValue = 65f;
    public float maxLensValue = 82f;
    public Vector3 zoomoutSpeedEffectPos;
    public Vector3 baseSpeedEffectPos;

    public float zoomDelay = 1f;

    public static PlayerFollowCam Instance;

    private CinemachineVirtualCamera _followCam;
    private CinemachineBasicMultiChannelPerlin m_channelsPerlin;
    private Transform speedParticle;

    [SerializeField]
    private float followCamFOVMultipleValue = 2;

    [SerializeField]
    private float totalMultipleValue;

    private bool isChecked = false;
    private void Awake()
    {
        Instance = this;

        _followCam = GetComponent<CinemachineVirtualCamera>();
        m_channelsPerlin = _followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        var player = GameObject.Find("Player");
        speedParticle = player.transform.Find("SpeedLineEffect");
    }

    private void Start()
    {
        _followCam.m_Lens.FieldOfView = minLensValue;
    }

    private void LateUpdate()
    {
        totalMultipleValue =
            (PlayerManager.Instance.GetMoveToForward.MoveSpeed / 10f) * followCamFOVMultipleValue;

        if (PlayerManager.Instance.GetMoveToForward.MoveSpeed >= startTargetValue)
        {
            if (isChecked == false)
            {
                //60이상이 넘었을때 한 번 실행(enter)
                StartCoroutine(CamZoom());
            }
            isChecked = true;
        }
        else
        {
            if (isChecked == true)
            {
                //60이상이 안 넘었을때 한 번 실행(exit)
                StopCoroutine(CamZoom());
                _followCam.m_Lens.FieldOfView = minLensValue;
                speedParticle.localPosition = baseSpeedEffectPos;
            }
            isChecked = false;
        }
    }

    public void ShakeTest()
    {
        ShakeCam(.5f, totalMultipleValue);
    }
    public void ShakeCam(float delay, float value)
    {
        StartCoroutine(CamShake(delay, value));
    }
    private IEnumerator CamZoom()
    {
        float startValue = _followCam.m_Lens.FieldOfView;
        float timer = 0f;

        Vector3 startPos = speedParticle.localPosition;
        while (timer < zoomDelay)
        {
            timer += Time.deltaTime;
            _followCam.m_Lens.FieldOfView = Mathf.Lerp(startValue, maxLensValue, timer / zoomDelay);
            speedParticle.localPosition = Vector3.Lerp(startPos, zoomoutSpeedEffectPos, timer / zoomDelay);

            yield return null;
        }
    }
    private IEnumerator CamShake(float delay, float value)
    {
        m_channelsPerlin.m_AmplitudeGain = value;
        m_channelsPerlin.m_FrequencyGain = value;
        yield return new WaitForSeconds(delay);
        m_channelsPerlin.m_AmplitudeGain = 0;
        m_channelsPerlin.m_FrequencyGain = 0;
    }

    private void OnDestroy()
    {
        _followCam.m_Lens.FieldOfView = minLensValue;
    }
}
