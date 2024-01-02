using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject Player { get; private set; }

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

    private MoveToForward _moveToForward;
    public MoveToForward GetMoveToForward
    {
        get
        {
            if (_moveToForward == null)
            {
                _moveToForward = Player.GetComponent<MoveToForward>();
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
                _buringSystem = Player.transform.Find("Visaul/Hips").GetComponent<BuringSystem>();
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
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
}
