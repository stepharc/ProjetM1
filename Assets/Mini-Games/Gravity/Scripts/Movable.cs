using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Color baseColor = Color.blue; // La couleur de base de l'objet
    private Color aimColor = Color.red; // La couleur lorsque l'objet est visé.
    private Vector3 positionBase;
    public Vector3 positionFin;
    public float vitesse = 0.1f;
    public bool moved = false;

	// Use this for initialization
	void Start ()
    {
        gameObject.tag = "Movable";
        positionBase = transform.position;
        GetComponent<MeshRenderer>().material.color = baseColor;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(moved) // Si moved, on déplace l'objet vers la position de fin.
            transform.position = Vector3.MoveTowards(transform.position, positionFin, vitesse);
        else // Sinon on retourne au début.
            transform.position = Vector3.MoveTowards(transform.position, positionBase, vitesse);
    }

    public void Aimed(bool aimed)
    {
        if (aimed) // Si l'objet est visé on change de couleur.
            GetComponent<MeshRenderer>().material.color = aimColor;
        else
            GetComponent<MeshRenderer>().material.color = baseColor;
    }

}
