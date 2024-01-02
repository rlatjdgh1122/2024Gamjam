using Cinemachine;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _targetObj;

    CinemachineVirtualCamera _followCam;

    void Awake()
    {
        _followCam = GetComponent<CinemachineVirtualCamera>();

        _followCam.Follow = _targetObj;

        //virtualCamera.m_XAxis.m_InputAxisName = "";
        //virtualCamera.m_YAxis.m_InputAxisName = ""; 
    }
}
