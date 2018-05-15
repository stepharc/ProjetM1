using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piques : MonoBehaviour
{
    public Transform sortie;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && (other.gameObject.GetComponent<Rigidbody>().useGravity || other.gameObject.GetComponent<GravityOrientation>() != null) && other.tag != "Character")
        {
            /* On tourne l'objet other dans le sens de sortie */
            other.transform.eulerAngles += -sortie.eulerAngles - transform.eulerAngles;
            /* On déplace l'objet */
            other.gameObject.transform.position = sortie.position;
            other.gameObject.transform.rotation = sortie.rotation;
            /* On réinitialise la vitesse de l'objet */
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
