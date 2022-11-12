using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerColor : MonoBehaviourPun
{
    //작성내용: 플레이어 액터넘버에 따라 캐릭터 디자인 바꿔줄 것

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            if(!photonView.IsMine)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            if(!photonView.IsMine)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }


    }

}
