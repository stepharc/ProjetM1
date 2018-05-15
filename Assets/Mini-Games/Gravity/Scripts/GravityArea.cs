using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
    public Vector3 orientation = Vector3.up;

    private void OnTriggerEnter(Collider other)
    {
        GravityOrientation go;
        // Lorsqu'un objet rentre dans la zone, on lui attache un GravityOrientation et on définit
        if(other.GetComponent<GravityOrientation>() != null)
        {
            go = other.GetComponent<GravityOrientation>();
        }
        else
        {
            go = other.gameObject.AddComponent<GravityOrientation>();
        }

        go.direction = orientation;
    }

    private void OnTriggerExit(Collider other)
    {
        /* On retire le GravityOrientation lorsque l'objet quitte la zone. */
        if (other.GetComponent<GravityOrientation>() != null)
        {
            Destroy(other.GetComponent<GravityOrientation>());
        }
    }
}
