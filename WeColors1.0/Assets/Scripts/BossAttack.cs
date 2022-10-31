using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BossAttack : MonoBehaviourPun
{
    //순서 bossAttack 바닥에 떨어짐 -> 떨어진 위치의 오브젝트 의 방향값으로 레이캐스트 생성
    // -> 시간에 따라 색이 변하도록 함

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
