using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    Game2Manager gameManager;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        gameManager.winner = other.gameObject;
    }
}
