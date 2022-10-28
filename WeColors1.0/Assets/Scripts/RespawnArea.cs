using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    [SerializeField] GameObject dontFall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            SettingRespawnArea();
        }
    }

    void SettingRespawnArea()
    {
        dontFall.GetComponent<Game2Respawn>().playerRespawnArea = transform.position;
    }
}
