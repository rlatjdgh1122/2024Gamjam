using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    private TextMeshProUGUI _speedText;
    public float _currentSpeed;
    [SerializeField] private float _speedChangeRate = 50f;

    public bool canSpeedText = false;

    private void Awake()
    {
        _speedText = GetComponent<TextMeshProUGUI>();
        _currentSpeed = 0f;
    }

    private void Update()
    {
        if(PlayerManager.Instance.IsDie)
        {
            return;
        }
        //if (!canSpeedText)
        //    _currentSpeed = Mathf.Lerp(_currentSpeed, PlayerManager.Instance.GetMoveToForward.MaxSpeed * 150 * 0.4f, Time.deltaTime * _speedChangeRate);
        if (canSpeedText)
            _currentSpeed = Mathf.Lerp(_currentSpeed, PlayerManager.Instance.GetMoveToForward.MoveSpeed * 150, Time.deltaTime);

        if (_currentSpeed < PlayerManager.Instance.GetMoveToForward.MaxSpeed * 150 * 0.4f && !canSpeedText)
        {
            _currentSpeed += Time.deltaTime * _speedChangeRate;
        }
        
        if (PlayerManager.Instance.GetMoveToForward.MoveSpeed >= (PlayerManager.Instance.GetMoveToForward.MaxSpeed * 0.4f))
        {
            canSpeedText = true;
        }

        _speedText.text = $"{_currentSpeed.ToString("N0")} au/s";
    }

}