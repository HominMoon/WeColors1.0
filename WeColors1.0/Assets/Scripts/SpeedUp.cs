using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Player") { return; }

        other.gameObject.GetComponent<PlayerMovement>().PlayerDash();
    }
}
