using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonGravity : MonoBehaviour
{
    private static MoonGravity instance = null;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }  
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static MoonGravity Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public float gravityForce = 1f;
}
