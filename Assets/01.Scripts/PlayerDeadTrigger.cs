using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadTrigger : MonoBehaviour
{
    public DieReasonType reasonType;
    public SFX sfxType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("²Î!");
            SoundManager.Instance.PlaySFXSound(sfxType);
            PlayerManager.Instance.GetPlayerDead.DeadImmedieatly();
            Invoke("ShowPanel", 1.5f);
        }
    }

    private void ShowPanel()
    {
        UIManager.Instance.DieReasonUI.UpdateIMG(reasonType);
    }
}
