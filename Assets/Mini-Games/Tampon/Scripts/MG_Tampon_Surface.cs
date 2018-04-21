using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Tampon_Surface : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Se déclenche quand le tampon est en contact avec la surface de jeu.
    //Note : Pour assurer le bon fonctionnement de cette méthode, nous avons été obligés d'ajouter des composants
    //Rigidbody aux objets Tampon et Surface, même si nous n'utilisons pas les propriétés physiques fournies.
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp;
        Vector3[] points = new Vector3[4];
        for(int i = 0; i < collision.contacts.Length; i++)
        {
            cp = collision.contacts[i];
            points[i] = cp.point;
            //Affiche le point de collision en noir sur la vue Scene.
            Debug.DrawRay(cp.point, cp.normal, Color.black, int.MaxValue, false);
        }
    }
}
