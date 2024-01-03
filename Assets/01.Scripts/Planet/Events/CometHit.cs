using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CometHit : MonoBehaviour
{
    [SerializeField] private Vector3 _maxSize;
    [SerializeField] float _duration;
    
    private GameObject Pluto;
    private int maxKeyCount = 20;
    private int currentkeyCount = 0;
    private float PlutoDis;
    private float clearTime = 4f;
    private float currentTime = 0;



    private void Start()
    {
        Pluto = GameObject.Find("Target");
    }

    private void Update()
    {
        PlutoDis = Vector3.Distance(transform.position, Pluto.transform.position);
        
        if (PlutoDis < 10f)
        {
            transform.DOScale(_maxSize, _duration);
            FastTyping();
        }
    }

    private void FastTyping()
    {
        if(currentTime < clearTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentkeyCount++;
            }

            if (currentkeyCount == maxKeyCount)
            {
                ClearGame();
            }
        }
    }

    void ClearGame()
    {
        //내용을 채우시오~
        Debug.Log("게임 클리어!");
    }
}
