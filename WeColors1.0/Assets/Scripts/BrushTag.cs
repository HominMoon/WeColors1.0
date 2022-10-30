using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrushTag : MonoBehaviourPun
{
    [SerializeField] GameObject brush;

    public void SetPlayerBrushTag()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            brush.tag = "Player1";
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            brush.tag = "Player2";
        }
    }
}
