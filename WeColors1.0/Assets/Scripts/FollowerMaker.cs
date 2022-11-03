using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FollowerMaker : MonoBehaviourPun
{
    [SerializeField] GameObject follower;
    [SerializeField] float maxDelay = 1f;
    [SerializeField] float minDelay = 0.5f;

    [SerializeField] int maxRange = 0;
    [SerializeField] int minRange = 15;
    
    float delayTime;


    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        StartCoroutine(InstantiateFollower());
    }

    IEnumerator InstantiateFollower()
    {
        delayTime = Random.Range(minDelay,maxDelay);
        PhotonNetwork.Instantiate(follower.name, 
            new Vector3(Random.Range(minRange, maxRange), 2 ,Random.Range(minRange, maxRange)),
            Quaternion.Euler(0,Random.Range(0,360),0));

        yield return new WaitForSeconds(delayTime);
        StartCoroutine(InstantiateFollower());
    }


}
