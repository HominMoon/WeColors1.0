using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Respawn : MonoBehaviour
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player") { return; }

        GameObject player = other.gameObject;

        player.transform.position = playerRespawnArea + new Vector3(4,2,0);
    }
}
