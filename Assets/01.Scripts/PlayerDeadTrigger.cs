using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("²Î!");
            PlayerManager.Instance.GetPlayerDead.DeadImmedieatly();
        }
    }
}
