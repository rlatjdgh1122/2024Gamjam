using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¶Ë");
            StageManager.Instance.MoveToNextStage();
            StageManager.Instance.CurrentStage++;
        }
    }
}
