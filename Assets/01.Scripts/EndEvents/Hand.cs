using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool canClap = false;
    [SerializeField] private float _dir;
    [SerializeField] private float _xDir;

    public void MoveAndRotate()
    {
        transform.DOMoveX(_xDir, 5f).OnComplete(() => transform.DORotate(new Vector3(0, _dir, 0), 1));
    }

    private void Update()
    {
        if (Vector3.Distance(transform.parent.position, PlayerManager.Instance.GetPlayerMovement.transform.position) <= 950 && !canClap)
        {
            MoveAndRotate();
            canClap = true;
        }
    }
}
