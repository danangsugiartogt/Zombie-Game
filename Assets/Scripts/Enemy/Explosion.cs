using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }
}
