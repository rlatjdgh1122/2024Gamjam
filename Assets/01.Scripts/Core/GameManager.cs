using TMPro;
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

    [SerializeField]
    private TextMeshProUGUI _timeText;

    public float curTime = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running! Check!");
        }
        Instance = this;
        MakePool();

    }

    private void Start()
    {
        curTime = 0;
        Debug.Log("start");
    }

    private void MakePool()
    {
        PoolManager.Instance = new PoolManager(transform);

        _poolingListSO.List.ForEach(p => PoolManager.Instance.CreatePool(p.prefab, p.poolCount)); //����Ʈ�� �ִ� ���
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

        if (!PlayerManager.Instance.IsDie)
        {
            curTime += Time.deltaTime;
        }

        int minutes = (int)curTime / 60;          // ��
        int remainingSeconds = (int)curTime % 60; // ��

        if (minutes > 0) { _timeText.SetText($"{minutes}: {remainingSeconds}"); } //������ ��Ÿ�� �� �ִٸ� �б��� ��Ÿ����.
        else { _timeText.SetText($"{remainingSeconds}"); }
    }
}
