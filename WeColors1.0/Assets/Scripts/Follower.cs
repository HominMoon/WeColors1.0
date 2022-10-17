using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Follower : MonoBehaviourPun
{
    int followingPlayerNumber;
    [SerializeField] GameObject followingPlayer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (followingPlayerNumber == 1)
        {
            transform.Translate(followingPlayer.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Painter" || other.gameObject.tag != "Player") { return; }

        // 게임 매니저에서 플레이어 목록 만들어서 목록 중 color로 판별
        
    }
}
