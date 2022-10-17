using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game2Respawn : MonoBehaviourPun
{
    private static Game2Respawn instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Game2Respawn Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public Vector3 playerRespawnArea;

    private void OnCollisionEnter(Collision other) {
        
        GameObject player = other.collider.gameObject;

        if(!player.GetPhotonView().IsMine) { return; }

        player.transform.Translate(playerRespawnArea);
        
        
    }
}
