using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporteur : MonoBehaviour
{
    public Transform sortie;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().useGravity && other.tag != "Character")
        {
            /* On tourne l'objet other dans le sens de sortie */
            other.transform.eulerAngles += -sortie.eulerAngles - transform.eulerAngles;
            /* On déplace l'objet */
            other.gameObject.transform.position = sortie.position;
            /* On conserve la vitesse de l'objet et on la dirige dans le sens de sortie */
            other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude * -sortie.forward.normalized;
        }
    }
}
