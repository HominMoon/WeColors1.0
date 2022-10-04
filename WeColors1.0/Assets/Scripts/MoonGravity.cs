using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonGravity : MonoBehaviour
{
    [SerializeField] float gravityForce = 1f;

    private void onTriggerStay(Collision other)
    {
        Debug.Log("iscollided");
        if(other.gameObject.tag == "Player")
        {
            GameObject playerObject = other.gameObject; 
            playerObject.GetComponent<Rigidbody>().AddForce(Vector3.up * gravityForce, ForceMode.Impulse);
        }
        
    }
}
