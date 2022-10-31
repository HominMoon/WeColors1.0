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

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(followingPlayer != null)
        {
           navMeshAgent.SetDestination(followingPlayer.transform.position); 
        }
    }

    // 플레이어가 휘둘러서 맞는 Painter의 태그가 1이냐 2냐에 따라 따라다니는 플레이어를 설정

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            followingPlayer = other.gameObject.transform.parent.gameObject;
            photonView.RPC("RPCSetFollowerColorRed", RpcTarget.All);
        }
        else if (other.gameObject.tag == "Player2")
        {
            followingPlayer = other.gameObject.transform.parent.gameObject;
            photonView.RPC("RPCSetFollowerColorBlue", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCSetFollowerColorRed()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    [PunRPC]
    void RPCSetFollowerColorBlue()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
