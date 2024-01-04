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
    [Header("적응 설정")]
    public float speedAdaptationTime = 3f;
    public float zoominDelay = .5f;
    public float adaptationLensValue = 80f;
    public Vector3 apdaptationSpeedEffectPos;
    [Header("가속도 설정")]
    public float minLensValue = 65f;
    public float maxLensValue = 100f;
    public float zoomoutDelay = 1f;
    public Vector3 zoomoutSpeedEffectPos;
    public Vector3 baseSpeedEffectPos;


    public static PlayerFollowCam Instance;

    private CinemachineVirtualCamera _followCam;
    public CinemachineVirtualCamera FollowCam => _followCam;
    private CinemachineBasicMultiChannelPerlin m_channelsPerlin;
    private CinemachineFramingTransposer m_body;
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
        m_body = _followCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        var player = GameObject.Find("Player");
        _followCam.m_Follow = player.transform;
        speedParticle = player.transform.Find("SpeedLineEffect");
    }

    private void Start()
    {
        _followCam.m_Lens.FieldOfView = minLensValue;
        speedParticle.localPosition = baseSpeedEffectPos;
        m_channelsPerlin.m_AmplitudeGain = 0;
        m_channelsPerlin.m_FrequencyGain = 0;
    }

    private void LateUpdate()
    {
        totalMultipleValue =
            (PlayerManager.Instance.GetMoveToForward.MoveSpeed / 10f) * followCamFOVMultipleValue;

        if (PlayerManager.Instance.GetMoveToForward.MoveSpeed >= (PlayerManager.Instance.GetMoveToForward.MaxSpeed * 0.4f))
        {
            if (isChecked == false)
            {
                //한 번 실행(enter)
                PlayerManager.Instance.GetBuringSystem.SetShotParticle();
                StartCoroutine(CamZoom());
                StartCoroutine(SpeedAdap());
            }
            isChecked = true;
        }
        else
        {
            if (isChecked == true)
            {
                //한 번 실행(exit)
                StopCoroutine(CamZoom());
                StopCoroutine(SpeedAdap());

                _followCam.m_Lens.FieldOfView = minLensValue;
                speedParticle.localPosition = baseSpeedEffectPos;
            }
            isChecked = false;
        }
    }

    public void IncreaseEyeSight(float value)
    {
        m_body.m_CameraDistance -= value;
    }

    public void ShakeTest()
    {
        ShakeCam(.5f, totalMultipleValue);
    }
    public void ShakeCam(float delay, float value)
    {
        StartCoroutine(CamShake(delay, value));
    }

    private IEnumerator SpeedAdap()
    {
        yield return new WaitForSeconds(speedAdaptationTime);

        float startValue = _followCam.m_Lens.FieldOfView;
        float timer = 0f;

        Vector3 startPos = speedParticle.localPosition;
        while (timer < zoominDelay)
        {
            timer += Time.deltaTime;
            _followCam.m_Lens.FieldOfView = Mathf.Lerp(startValue, adaptationLensValue, timer / zoominDelay);
            speedParticle.localPosition = Vector3.Lerp(startPos, apdaptationSpeedEffectPos, timer / zoominDelay);

            yield return null;
        }
        _followCam.m_Lens.FieldOfView = adaptationLensValue;
        speedParticle.localPosition = apdaptationSpeedEffectPos;
    }
    private IEnumerator CamZoom()
    {
        float startValue = _followCam.m_Lens.FieldOfView;
        float timer = 0f;

        Vector3 startPos = speedParticle.localPosition;
        while (timer < zoomoutDelay)
        {
            timer += Time.deltaTime;
            _followCam.m_Lens.FieldOfView = Mathf.Lerp(startValue, maxLensValue, timer / zoomoutDelay);
            speedParticle.localPosition = Vector3.Lerp(startPos, zoomoutSpeedEffectPos, timer / zoomoutDelay);

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
