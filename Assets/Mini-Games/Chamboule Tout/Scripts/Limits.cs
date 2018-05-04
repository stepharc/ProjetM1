using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    /* Un objet est détruit quand il sort des limites du jeu. */
    void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
