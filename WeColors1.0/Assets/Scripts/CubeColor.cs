using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CubeColor : MonoBehaviourPun
{
    MeshRenderer rd;

    void start()
    {
        rd = GetComponent<MeshRenderer>();
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player1" && GetComponent<MeshRenderer>().material.color != Color.red)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (other.gameObject.tag == "Player2" && GetComponent<MeshRenderer>().material.color != Color.blue)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

}
