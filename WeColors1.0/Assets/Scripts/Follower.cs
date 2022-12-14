using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviourPun
{
    [SerializeField] float followingRange = 5f;
    [SerializeField] GameObject followingPlayer = null;
    [SerializeField] NavMeshAgent navMeshAgent;

    int followingPlayerNum = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followingPlayer == null) { return; }
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(followingPlayer.transform.position);
    }

    // 플레이어가 휘둘러서 맞는 Painter의 태그가 1이냐 2냐에 따라 따라다니는 플레이어를 설정

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            followingPlayer = other.transform.parent.gameObject;
            photonView.RPC("RPCSetFollowerColorRed", RpcTarget.All);
        }
        else if (other.gameObject.tag == "Player2")
        {
            followingPlayer = other.transform.parent.gameObject;
            photonView.RPC("RPCSetFollowerColorBlue", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCSetFollowerColorRed()
    {
        followingPlayerNum = 1;
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    [PunRPC]
    void RPCSetFollowerColorBlue()
    {
        followingPlayerNum = 2;
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
