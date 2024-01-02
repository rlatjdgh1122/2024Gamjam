using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    private TextMeshProUGUI _speedText;

    [SerializeField] private PlayerFowardMover _player;

    private void Awake()
    {
        _speedText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _speedText.text = $"{_player.MoveSpeed * 200} km/s";
    }
}