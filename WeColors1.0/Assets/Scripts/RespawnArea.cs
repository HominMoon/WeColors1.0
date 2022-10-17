using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RespawnArea : MonoBehaviourPun
{
    private static RespawnArea instance = null;

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

    public static RespawnArea Instance
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


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetPhotonView().IsMine) { return; }

        if (other.gameObject.tag == "Player")
        {
            SettingRespawnArea();
        }
    }

    void SettingRespawnArea()
    {
        Game2Respawn.Instance.playerRespawnArea = transform.position;
    }
}
