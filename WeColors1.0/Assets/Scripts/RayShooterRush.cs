using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RayShooterRush : MonoBehaviourPun
{
    [SerializeField] RaycastHit[] raycastHits;


    public void DoRay()
    {
        raycastHits = Physics.RaycastAll(gameObject.transform.position, gameObject.transform.forward, Mathf.Infinity);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            raycastHit.collider.gameObject.GetComponent<CubeColorRush>().ChangeColor();
        }

        Destroy(this.gameObject);
    }
}
