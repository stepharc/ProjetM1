using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityOrientation : MonoBehaviour
{
    private float force = Physics.gravity.magnitude; // La force de la gravité par défaut.
    public Vector3 direction = Vector3.down; // La direction de la nouvelle gravité.

    // Use this for initialization
    void Start()
    {
        direction.Normalize(); // On limite la gravité à la direction uniquement.
        GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity += direction * force * Time.deltaTime; // On augmente la vitesse de chute avec le temps.
    }

    private void OnDestroy()
    {
        GetComponent<Rigidbody>().useGravity = true; // On remet la gravité normale à la fin.
    }
}
