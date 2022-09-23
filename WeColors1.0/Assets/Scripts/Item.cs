using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviourPun
{
    // 아이템이 공통적으로 가지는 특성 기술, 부딪힌 상대 플레이어 넘버 감지 등

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Player") { return; }
        
        Destroy(gameObject);

    }
}
