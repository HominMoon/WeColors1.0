using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemBomby : MonoBehaviourPun
{
    //순서: 플레이어가 아이템 먹음 -> 레이 발사할 오브젝트 생성 (생성위치 중요) -> 레이 발사해 닿은 오브젝트들 changeColor
    //현재 문제: 플레이어가 두 명이라 2개씩 실행됨, 아이템을 먹은 플레이어가 아닌경우 실행되지 않게 해야한다.
    [SerializeField] GameObject rayShooter;

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.GetPhotonView().IsMine)
        {
            return;
        }
        GameObject shooter = Instantiate(rayShooter,new Vector3(gameObject.transform.position.x, 0 , -1),
         Quaternion.Euler(0,0,0));
        shooter.GetComponent<RayShooter>().DoRay();
    }
}
