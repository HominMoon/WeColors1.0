using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RayShooterRush : MonoBehaviourPun
{
    [SerializeField] RaycastHit[] raycastHits;

    [SerializeField] float delayTime = 0.2f;

    private IEnumerator delayCoroutine;

    public void DoRay()
    {
        raycastHits = Physics.RaycastAll(gameObject.transform.position, gameObject.transform.forward, Mathf.Infinity);

        Array.Sort(raycastHits,(RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));

        delayCoroutine = DelayRayCastRush();
        StartCoroutine(delayCoroutine);
    }

    IEnumerator DelayRayCastRush()
    {
        for(int i=0;i < raycastHits.Length ; i++)
        {
            yield return new WaitForSeconds(delayTime);
            raycastHits[i].collider.gameObject.GetComponent<CubeColorRush>().ChangeColor();
        }

        StopCoroutine(delayCoroutine);
    }
}
