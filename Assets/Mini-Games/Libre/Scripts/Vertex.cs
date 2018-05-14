using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex : MonoBehaviour
{
    public bool selected;
    private List<Joycon> joycons;
    private Joycon j;
    private float[] controls = { 0, 0, 0 }; // Coordonnées des axes de controles.
    public float[] stick;
    public float vitesse = 0.1f;
    public float limit = 1f; // Une limite sur l'axe y pour la hauteur du curseur.
    public int jc_ind = 0;
    /* La liste des références des sommets communs. */
    public List<int> vertices = new List<int>();

    void Start()
    {
        selected = false;
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
        }
    }

    void Update()
    {
        if (selected)
        {
            if(joycons.Count > 0)
            {
                DeplacementJoycon();
            }
            else
            {
                DeplacementAutreControle();
            }
        }
    }

    public void Add(int id)
    {
        vertices.Add(id);
    }

    public void setSelected(bool s)
    {
        selected = s;
    }

    public bool isSelected()
    {
        return selected;
    }

    void DeplacementJoycon()
    {
        stick = j.GetStick();

        if (j.isLeft)
        {
            controls[0] = -stick[1] * vitesse;
            controls[2] = stick[0] * vitesse;

            /* Ces boutons pour monter ou descendre */
            if (j.GetButton(Joycon.Button.DPAD_RIGHT) && transform.localPosition.y < limit)
            {
                controls[1] = vitesse;
            }
            else if (j.GetButton(Joycon.Button.DPAD_LEFT) && transform.localPosition.y > -limit)
            {
                controls[1] = -vitesse;
            }
            else
            {
                controls[1] = 0;
            }
        }
        else
        {
            controls[0] = stick[1] * vitesse;
            controls[2] = -stick[0] * vitesse;

            /* Ces boutons pour monter ou descendre */
            if (j.GetButton(Joycon.Button.DPAD_LEFT) && transform.localPosition.y < limit)
            {
                controls[1] = vitesse;
            }
            else if (j.GetButton(Joycon.Button.DPAD_RIGHT) && transform.localPosition.y > -limit)
            {
                controls[1] = -vitesse;
            }
            else
            {
                controls[1] = 0;
            }
        }

        /* Le sommet bouge dans le domaine local, donc par rapport à son objet parent et non par rapport au monde. */
        transform.localPosition += new Vector3(controls[0], controls[1], controls[2]);
    }

    void DeplacementAutreControle()
    {
        /* Fonctions basiques pour les claviers et manettes classiques. */
        controls[0] = Input.GetAxis("Horizontal") * vitesse;
        controls[2] = Input.GetAxis("Vertical") * vitesse;

        /* Ces boutons pour monter ou descendre */
        if (Input.GetButton("Jump") && transform.localPosition.y < limit)
        {
            controls[1] = vitesse;
        }
        else if (Input.GetButton("Fire1") && transform.localPosition.y > -limit)
        {
            controls[1] = -vitesse;
        }
        else
        {
            controls[1] = 0;
        }

        transform.localPosition += new Vector3(controls[0], controls[1], controls[2]);
    }
}
