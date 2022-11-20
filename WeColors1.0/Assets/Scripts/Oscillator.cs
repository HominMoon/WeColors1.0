using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Oscillator : MonoBehaviourPun
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;


    void Start()
    {
        startingPosition = transform.position; //current position
    }

    void Update()
    {
        float cycles;
        if (period == Mathf.Epsilon) //floating point 값을 다른 floating point와 비교하지 않기위해.
            return;

        cycles = Time.time / period; // continually growing over time
        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); // -1 ~ +1

        movementFactor = (rawSinWave + 1f) / 2f; // 0 ~ 1

        Vector3 offset = movementVector * movementFactor;

        //if(!PhotonNetwork.IsMasterClient) { return; }

        transform.position = startingPosition + offset;
    }
}

//two floats can vart by a tiny amount
// Mathf.Epsilon -> very tiny number close to 0
