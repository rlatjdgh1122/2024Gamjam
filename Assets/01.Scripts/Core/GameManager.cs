using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pooling")]
    [SerializeField]
    PoolingListSO _poolingListSO;

    [Header("Game Stat")]
    public float distanceToEarth = 100;

    [SerializeField]
    private PlayerFowardMover _mover;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running! Check!");
        }
        Instance = this;
        MakePool();
    }

    private void MakePool()
    {
        PoolManager.Instance = new PoolManager(transform);

        _poolingListSO.List.ForEach(p => PoolManager.Instance.CreatePool(p.prefab, p.poolCount)); //리스트에 있는 모든
    }

    private void Update()
    {
        if (_mover.IsEnable)
        {
            float speed = _mover.MoveSpeed;

            float decreaseAmount = speed / distanceToEarth;
            distanceToEarth -= decreaseAmount * Time.deltaTime;

            distanceToEarth = Mathf.Max(0, distanceToEarth);
        }
    }
}
