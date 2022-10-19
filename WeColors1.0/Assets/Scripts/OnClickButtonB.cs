using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class OnClickButtonB : MonoBehaviourPun
{
    GameObject[] players;
    GameObject myPlayer;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(3.5f);
        players = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i< players.Length ; i++)
        {
            if(players[i].gameObject.GetPhotonView().IsMine)
            {
                myPlayer = players[i];
                break;
            }
        }

        gameObject.GetComponent<Button>().onClick.AddListener(myPlayer.GetComponent<PlayerMovement>().Attack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
