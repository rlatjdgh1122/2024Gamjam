using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    //크기가 크면 크기를 많이 줄여주고 온도를 많이 낮줘줌
    public int damage = 0; //플레이어의 크기를 얼마나 줄여줄지

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어의 크기를 줄여줌
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }
}
