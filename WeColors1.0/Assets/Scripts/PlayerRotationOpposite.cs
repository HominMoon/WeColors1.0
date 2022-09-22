using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRotationOpposite : MonoBehaviour
{
    //요약: 플레이어2가 플레이어1과 마주볼 수 있도록 돌려준다.

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
