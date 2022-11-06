using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AttackDamage : MonoBehaviourPun
{
    [SerializeField] GameObject game4Manager;

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.GetComponent<MeshRenderer>().material.color != Color.black) { return; }

        if (other.gameObject.GetComponent<MeshRenderer>().material.color.a != 1f) { return; }

        if (other.gameObject.tag == "Player" && other.gameObject.GetPhotonView().IsMine)
        {
            StartCoroutine(PlayerInvincivle(other));
            game4Manager.GetComponent<Game4Manager>().PlayerHealth();
        }
    }

    IEnumerator PlayerInvincivle(Collision other)
    {
        PlayerDamaged();
        Color color = other.gameObject.GetComponent<MeshRenderer>().material.color;
        color.a = 0.5f;

        yield return new WaitForSeconds(3f);
        PlayerRecovery();
        color.a = 1f;
    }

    private void PlayerDamaged()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            game4Manager.GetComponent<Game4Manager>().isPlayer1Damaged = true;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            game4Manager.GetComponent<Game4Manager>().isPlayer2Damaged = true;
        }
    }

    private void PlayerRecovery()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            game4Manager.GetComponent<Game4Manager>().isPlayer1Damaged = false;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            game4Manager.GetComponent<Game4Manager>().isPlayer2Damaged = false;
        }
    }
}
