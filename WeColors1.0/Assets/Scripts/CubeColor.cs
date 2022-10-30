using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColor : MonoBehaviourPun
{
    //요약: 플레이어 오브젝트에서 충돌 판정이 들어올 경우 실행될 메서드,
    // RPC로 상대와 블럭 색깔이 동기화 되도록 한다.

    public void ChangeColor(int playerNum)
    {
        photonView.RPC("RPCChangeColor", RpcTarget.All, playerNum);
    }

    [PunRPC]
    public void RPCChangeColor(int playerNum)
    {
        if (playerNum == 1)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        else if (playerNum == 2)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }

    }

    //**폐기 코드**

    // public bool IsMasterClient()
    // {
    //     if(PhotonNetwork.IsMasterClient && photonView.IsMine) { return true; }

    //     return false;
    // }


    // void OnCollisionEnter(Collision other) 
    // {
    //     if(other.gameObject.tag != "Player") { return; }

    //     photonView.RPC("RPCChangeColor", RpcTarget.All);
    // }

    // [PunRPC]
    // void RPCChangeColor()
    // {
    //     if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
    //     { 
    //         mr.material.color = Color.red;
    //     }
    //     else
    //     {
    //         mr.material.color = Color.blue;
    //     }
    // }

}
