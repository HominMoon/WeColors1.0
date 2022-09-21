using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColor : MonoBehaviourPun
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public void ChangeColor(int playerNum)
    {
        photonView.RPC("RPCChangeColor", RpcTarget.All, playerNum);
    }

    [PunRPC]
    void RPCChangeColor(int playerNum)
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
