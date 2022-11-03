using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossAttackSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject[] bossAttackSpawner;

    [SerializeField] float minSpawnRange = 2f;
    [SerializeField] float maxSpawnRange = 3f;

    int objectRange;

    int spwanRandomPosition;
    float spawnRandomTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        StartCoroutine(Spawn());
        objectRange = bossAttackSpawner.Length;
    }

    IEnumerator Spawn()
    {
        spawnRandomTime = Random.Range(minSpawnRange, maxSpawnRange);
        spwanRandomPosition = Random.Range(0,objectRange - 1);
        yield return new WaitForSeconds(spawnRandomTime);
        PhotonNetwork.Instantiate(Ball.name, bossAttackSpawner[spwanRandomPosition].transform.position, transform.rotation);
        //PhotonNetwork.Instantiate(Ball.name, transform.position, transform.rotation); // -> 실제 사용
        StartCoroutine(Spawn());
    }

}
