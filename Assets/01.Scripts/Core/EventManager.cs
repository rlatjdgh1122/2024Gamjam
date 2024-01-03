using UnityEngine;

[System.Serializable]
public class EventProbability
{
    public GameObject eventObject;
    [Range(0f, 1f)] public float probability;
}

public class EventManager : MonoBehaviour
{
    [SerializeField] private EventProbability[] _eventProbabilities;

    private void Awake()
    {
        SelectRandomEvent();
    }

    private void SelectRandomEvent()
    {
        if (_eventProbabilities.Length == 0)
        {
            Debug.LogError("No events in the array.");
            return;
        }

        // ��ü Ȯ�� �� ���
        float totalProbability = 0f;
        foreach (EventProbability eventProbability in _eventProbabilities)
        {
            totalProbability += eventProbability.probability;
        }

        // 0���� 1 ������ ������ Ȯ�� �� ����
        float randomValue = Random.Range(0f, totalProbability);

        // ������ Ȯ�� ���� �ش��ϴ� �̺�Ʈ ����
        float cumulativeProbability = 0f;
        GameObject selectedEvent = null;

        foreach (EventProbability eventProbability in _eventProbabilities)
        {
            cumulativeProbability += eventProbability.probability;

            if (randomValue <= cumulativeProbability)
            {
                selectedEvent = eventProbability.eventObject;
                break;
            }
        }

        if (selectedEvent != null)
        {
            // ���õ� �̺�Ʈ Ȱ��ȭ
            selectedEvent.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to select a random event.");
        }
    }
}
