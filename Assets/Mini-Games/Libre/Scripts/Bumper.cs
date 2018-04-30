using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float force = 20f;

    /* Lorsqu'un objet rentre en contact avec le Bumper il est propulsé */
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().useGravity)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward*force* other.gameObject.GetComponent<Rigidbody>().mass);
        }
    }
}
