using UnityEngine;
using System.Collections;

public class End : MonoBehaviour
{
    private Color col;
    // Use this for initialization
    void Start()
    {
        col = GetComponent<Renderer>().material.color; // La couleur de base de l'objet.
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Change de couleur lorsque la balle arrive à la fin */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Ball")
        {
            gameObject.GetComponent<Renderer>().material.color = col;
        }
    }
}