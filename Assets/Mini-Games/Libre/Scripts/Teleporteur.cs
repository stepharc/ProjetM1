using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporteur : MonoBehaviour
{
    public Transform sortie;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().useGravity)
        {
            other.gameObject.transform.position = sortie.position;
        }
    }
}
