using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RayShooter : MonoBehaviourPun
{
    [SerializeField] RaycastHit[] raycastHits;
    int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

    public void DoRay()
    {
        raycastHits = Physics.RaycastAll(gameObject.transform.position, gameObject.transform.forward, Mathf.Infinity);

        foreach(RaycastHit raycastHit in raycastHits)
        {
            raycastHit.collider.gameObject.GetComponent<CubeColor>().ChangeColor(playerNum);
        }

        Destroy(this.gameObject);
    }
}
