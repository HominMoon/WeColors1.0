using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Spinner : MonoBehaviourPun
{
    [SerializeField] float xval = 1f;
    [SerializeField] float yval = 1f;
    [SerializeField] float zval = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(!PhotonNetwork.IsMasterClient) { return; }

        transform.Rotate(new Vector3(
        xval * Time.deltaTime, 
        yval * Time.deltaTime, 
        zval * Time.deltaTime));
    }
}
