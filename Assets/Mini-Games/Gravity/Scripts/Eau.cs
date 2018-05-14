using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eau : MonoBehaviour
{
    public float force = 4;
    /*On augmente la force lorsque l'objet rentre dans la zone. */
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().drag = force;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().drag = 0;
        }
    }
}
