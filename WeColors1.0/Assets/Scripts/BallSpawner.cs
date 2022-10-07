using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject Ball;

    [SerializeField] float minSpawnRange = 2f;
    [SerializeField] float maxSpawnRange = 3f;
    

    float spawnRandomTime;

    // Start is called before the first frame update
    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient) { return; }
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        spawnRandomTime = Random.Range(minSpawnRange, maxSpawnRange);
        yield return new WaitForSeconds(spawnRandomTime);
        Instantiate(Ball, transform.position, transform.rotation);
        //PhotonNetwork.Instantiate(Ball.name, transform.position, transform.rotation); // -> 실제 사용
        StartCoroutine(Spawn());
    }

}
