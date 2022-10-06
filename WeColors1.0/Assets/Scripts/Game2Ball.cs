using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
