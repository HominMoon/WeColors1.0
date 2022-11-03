using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColorRush : MonoBehaviourPun
{
    [SerializeField] float undoSpeed = 1f;
    [SerializeField] float popPower = 5f;

    Rigidbody rd;

    public void ChangeColor()
    {
        StartCoroutine(DelayForRush());
    }

    IEnumerator DelayForRush()
    {
        photonView.RPC("RPCChangeColor", RpcTarget.All);
        yield return new WaitForSeconds(undoSpeed);
        photonView.RPC("RPCUndoColor", RpcTarget.All);
    }

    [PunRPC]
    public void RPCChangeColor()
    {
        gameObject.tag = "BossAttack";
        rd = GetComponent<Rigidbody>();
        rd.AddForce(Vector3.up * popPower , ForceMode.Impulse);
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    [PunRPC]
    public void RPCUndoColor()
    {
        gameObject.tag = "Untagged";
        GetComponent<MeshRenderer>().material.color = new Color(229/255f,229/255f,229/255f);
    }
}
