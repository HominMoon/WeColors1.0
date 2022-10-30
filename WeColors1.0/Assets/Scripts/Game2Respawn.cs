using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Respawn : MonoBehaviour
{
    public Vector3 playerRespawnArea;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player") { return; }

        GameObject player = other.gameObject;

        player.transform.position = playerRespawnArea + new Vector3(4,2,0);
    }
}
