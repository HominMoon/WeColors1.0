using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviourPun
{
    int followingPlayerNumber;
    [SerializeField] float followingRange = 5f; 
    [SerializeField] GameObject followingPlayer;
    [SerializeField] NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(followingPlayer.transform.position);
    }

    // 플레이어가 휘둘러서 맞는 Painter의 태그가 1이냐 2냐에 따라 따라다니는 플레이어를 설정

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player1")
        {
            followingPlayer = Game3Manager.Instance.playerList[0];
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if(other.gameObject.tag == "Player2")
        {
            followingPlayer = Game3Manager.Instance.playerList[1];
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }
}
