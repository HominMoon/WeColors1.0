using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Respawn : MonoBehaviour
{
    Transform player1RespawnArea;
    Transform player2RespawnArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 각 플레이어가 아치를 지났을 때 마다 업데이트 되어야 한다.
        // 업데이트 방법 -> 플레이어가 지역을 지나감 -> 리스폰 지역이 해당 장소로 업데이트 됨.
    }

    private void OnCollisionEnter(Collision other) {
        
        GameObject player = other.collider.gameObject;
        
        player.transform.Translate(RespawnArea.Instance.SettingRespawnArea());
    }
}
