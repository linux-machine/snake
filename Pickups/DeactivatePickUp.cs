using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Deactivate", Random.Range(3.0f, 6.0f));
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
