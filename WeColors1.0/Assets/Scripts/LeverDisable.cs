using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDisable : MonoBehaviour
{
    [SerializeField] GameObject lever;
    [SerializeField] GameObject ButtonA;
    [SerializeField] GameObject ButtonB;

    public void SetLeverDisable()
    {
        lever.gameObject.SetActive(false);
        ButtonA.gameObject.SetActive(false);
        ButtonB.gameObject.SetActive(false);
    }
}
