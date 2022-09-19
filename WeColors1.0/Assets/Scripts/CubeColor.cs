using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColor : MonoBehaviourPun
{
    MeshRenderer rd;

    void start()
    {
        rd = GetComponent<MeshRenderer>();
    }

    public bool IsMasterClient()
    {
        if(PhotonNetwork.IsMasterClient && photonView.IsMine) { return true; }
        
        return false;
    }

    [PunRPC]
    void OnCollisionEnter(Collision other) 
    {
        if(!IsMasterClient() || PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        if(other.gameObject.tag == "Player1" && GetComponent<MeshRenderer>().material.color != Color.red)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (other.gameObject.tag == "Player2" && GetComponent<MeshRenderer>().material.color != Color.blue)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

}
