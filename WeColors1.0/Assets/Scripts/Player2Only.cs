using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player2Only : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            //transform.rotation = Quaternion.Euler(0,180,0);    
        }

    }
}
