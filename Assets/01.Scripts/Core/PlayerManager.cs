using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject Player { get; private set; }

    private bool isDie;
    public bool IsDie
    {
        get
        {
            if (GetBuringSystem.BurningValue >= GetBuringSystem.MaxBurningValue) // 내구도 관련해서도 추가해야됨
            {
                isDie = true;
            }
            return isDie;
        }
    }

    private PlayerMovement _playerMovement;
    public PlayerMovement GetPlayerMovement
    {
        get
        {
            if (_playerMovement == null)
            {
                _playerMovement = Player.GetComponent<PlayerMovement>();
            }
            return _playerMovement;
        }
    }

    private PlayerFowardMover _moveToForward;
    public PlayerFowardMover GetMoveToForward
    {
        get
        {
            if (_moveToForward == null)
            {
                _moveToForward = Player.GetComponent<PlayerFowardMover>();
            }
            return _moveToForward;
        }
    }

    private DurabilitySystem _durabilitySystem;
    public DurabilitySystem GetDurabilitySystem
    {
        get
        {
            if (_durabilitySystem == null)
            {
                _durabilitySystem = Player.GetComponent<DurabilitySystem>();
            }
            return _durabilitySystem;
        }
    }

    private PlayerSizeSystem _playerSizeSystem;
    public PlayerSizeSystem GetPlayerSizeSystem
    {
        get
        {
            if (_playerSizeSystem == null)
            {
                _playerSizeSystem = Player.GetComponent<PlayerSizeSystem>();
            }
            return _playerSizeSystem;
        }
    }

    private PlayerRagdoll _playerRagdoll;
    public PlayerRagdoll GetPlayerRagdoll
    {
        get
        {
            if (_playerRagdoll == null)
            {
                _playerRagdoll = Player.GetComponent<PlayerRagdoll>();
            }
            return _playerRagdoll;
        }
    }

    private BuringSystem _buringSystem;
    public BuringSystem GetBuringSystem
    {
        get
        {
            if (_buringSystem == null)
            {
                //_buringSystem = Player.transform.Find("Visual/Hips").GetComponent<BuringSystem>();
                _buringSystem = GameObject.FindObjectOfType<BuringSystem>();
            }
            return _buringSystem;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");

        }
        Instance = this;

        Player = GameObject.FindWithTag("Player");
    }
}
