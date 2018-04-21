using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Tampon_Surface : MonoBehaviour {
    private Color tColor;
    private float tRotateY, tScaleX, tScaleZ;
    private string tName;

    //Récupère la couleur du tampon pour que la couleur des traces qu'il laisse soit identique avec le tampon.
    public void setTrackColor(Color c)
    {
        tColor = c;
    }

    //Récupère l'axe de rotation Y du tampon.
    public void setTamponRotation(float f)
    {
        tRotateY = f;
    }

    //Récupère les dimensions X et Z du tampon.
    public void setTamponScales(float fx, float fz)
    {
        tScaleX = fx;
        tScaleZ = fz;
    }

    //Récupère le nom de l'objet représentant le tampon.
    public void setTamponName(string s)
    {
        tName = s;
    }

    //Affiche la marque laissée par le tampon lors de son déplacement sur la surface de jeu.
    private void drawTrack(Collision collision)
    {
        //Coordonnée y de la face supérieure (sur laquelle se trouve le tampon) de la surface de jeu.
        float ySurface = GetComponent<Renderer>().bounds.max.y;
        GameObject track = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //On place la trace de telle sorte qu'elle couvre toute la zone de collision
        track.transform.position = collision.collider.bounds.center;
        //La coordonnée y est changée pour que la trace se trouve pile au dessus de la surface de jeu.
        track.transform.position = new Vector3(track.transform.position.x, ySurface, track.transform.position.z);
        //On oriente la trace de telle sorte qu'elle corresponde à celle du tampon à la fin de son saut.
        track.transform.Rotate(new Vector3(0, tRotateY, 0));
        //On désactive les collisions de la trace pour éviter que le tampon ne se trouve au dessus de celle-ci, évitant ainsi la
        //rupture de contact entre le tampon et la surface de jeu.
        track.GetComponent<Collider>().enabled = false;
        track.GetComponent<Renderer>().material.color = tColor;
        track.transform.localScale = new Vector3(tScaleX, 0.025f, tScaleZ);
        //La trace créée ne sera pas visible dans l'onglet Hiérarchie sur Unity3D.
        track.hideFlags = HideFlags.HideInHierarchy;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Exécute le code de cette fonction quand le tampon est en contact avec la surface de jeu.
    //Note : Pour assurer le bon fonctionnement de cette méthode, nous avons été obligés d'ajouter des composants
    //Rigidbody aux objets Tampon et Surface, même si nous n'utilisons pas les propriétés physiques fournies.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == tName)
        {
            ContactPoint cp;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                cp = collision.contacts[i];
                Debug.Log("Point " + i + " : " + cp.point);
                //Affiche le point de collision en noir sur la vue Editeur.
                Debug.DrawRay(cp.point, cp.normal, Color.black, int.MaxValue, false);
            }
            drawTrack(collision);
        }
    }
}
