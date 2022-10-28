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
        
    }

    public void SetPlayerTag()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            gameObject.tag = "Player1";
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            gameObject.tag = "Player2";
        }
    }
}
