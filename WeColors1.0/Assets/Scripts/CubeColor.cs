using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColor : MonoBehaviourPun
{
    MeshRenderer mr;

    void start()
    {
        mr = GetComponent<MeshRenderer>();
    }

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
