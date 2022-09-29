using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaitManager : MonoBehaviourPun
{
    void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("tn:" + PhotonNetwork.PlayerList.Length);
        PhotonNetwork.AutomaticallySyncScene = true;
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game2");
        }
        
    }

}
