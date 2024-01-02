using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [Header("플레이어에게 영향을 줄 스탯")]
    //크기가 크면 크기를 많이 줄여주고 온도를 많이 낮줘줌
    public int damage = 0; //플레이어의 크기를 얼마나 줄여줄지
    public int lowerTem = 0; //플레이어의 온도를 얼마나 낮춰줄건지
    public int lowerSpeed = 0; //플레이어의 속도를 얼마나 낮춰줄건지
    [Header("내 스탯")]
    public int MaxSpeed; //움직임의 속도
    private Vector2 dir; //움직일 방향

    private float speed = 0;
    private void Start()
    {
        speed = Random.Range(0, (float)MaxSpeed); //랜덤으로 스피드
        dir = Random.insideUnitSphere * 1f; //랜덤 방향
    }

    private void Update()
    {
        //움직이는 코드
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        transform.Rotate(dir.normalized, speed * 50f * Time.deltaTime);
        //회전하는 코드
    }
    private void OnCollisionEnter(Collision collision)
    {
        //플레이어의 온도, 속도를 줄여주고 크기를 줄여줌
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }
}
