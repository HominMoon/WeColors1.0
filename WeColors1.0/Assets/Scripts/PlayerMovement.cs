using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    Rigidbody rd;
    Transform tr;

    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        float xVal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float zVal = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(xVal,0,zVal);

    }
}
