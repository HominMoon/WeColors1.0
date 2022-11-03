using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StartRush : MonoBehaviourPun
{
    [SerializeField] GameObject rayShooter;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != "BossAttack" || !PhotonNetwork.IsMasterClient)
        {
            return;
        }

        GameObject shooter = PhotonNetwork.Instantiate(rayShooter.name, gameObject.transform.position, gameObject.transform.rotation);

        shooter.GetComponent<RayShooterRush>().DoRay();
    }
}
