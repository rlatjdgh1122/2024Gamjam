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

        // 전체 확률 합 계산
        float totalProbability = 0f;
        foreach (EventProbability eventProbability in _eventProbabilities)
        {
            totalProbability += eventProbability.probability;
        }

        // 0에서 1 사이의 무작위 확률 값 생성
        float randomValue = Random.Range(0f, totalProbability);

        // 무작위 확률 값에 해당하는 이벤트 선택
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
            // 선택된 이벤트 활성화
            selectedEvent.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to select a random event.");
        }
    }
}
