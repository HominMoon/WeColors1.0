using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColorRush : MonoBehaviourPun
{
    [SerializeField] float delayTime = 0.1f;
    [SerializeField] float undoSpeed = 0.5f;

    public void ChangeColor()
    {
        StartCoroutine(DelayForRush());
    }

    IEnumerator DelayForRush()
    {
        yield return new WaitForSeconds(delayTime);
        photonView.RPC("RPCChangeColor", RpcTarget.All);
        StartCoroutine(UndoColor());
    }

    IEnumerator UndoColor()
    {
        yield return new WaitForSeconds(undoSpeed);
        photonView.RPC("RPCUndoColor", RpcTarget.All);
    }

    [PunRPC]
    public void RPCChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = Color.gray;
    }

    [PunRPC]
    public void RPCUndoColor()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
