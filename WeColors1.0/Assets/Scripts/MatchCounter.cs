using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class MatchCounter : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Invoke("PhotonLoadLevel", 3f);
    }

    private static void PhotonLoadLevel()
    {
        PhotonNetwork.LoadLevel("Game1");
    }
}
