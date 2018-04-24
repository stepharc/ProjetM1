using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Curseur : MonoBehaviour
{
    private enum Modes : int
    {
        DEPLACEMENT = 1, // Pour déplacer l'objet
        OBSERVATION = 2, // Pour observer
        MODIFICATION = 3 // Pour modifier si autoriser
    };
    private List<Joycon> joycons;
    private Joycon j;
    private float[] controls = { 0, 0, 0 }; // Coordonnées des axes de controles.
    private Color couleurBase;
    public GameObject selection; // L'objet selectionné par le curseur.
    [Range(1, 3)]
    public int mode;
    public float[] stick;
    public float vitesse;
    public float limit; // Une limite sur l'axe y pour la hauteur du curseur.
    public int jc_ind = 0;

    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
        }
    }

    void Update()
    {
        if (joycons.Count > 0)
        {
            DeplacementJoycon();

        }
        else
        {
            DeplacementAutreControl();
        }

        Lacher();
    }

    void DeplacementJoycon()
    {
        if (mode == (int)Modes.DEPLACEMENT)
        {
            stick = j.GetStick();

            controls[0] = -stick[1] * vitesse;
            controls[2] = stick[0] * vitesse;

            /* Ces boutons pour monter ou descendre */
            if (j.GetButton(Joycon.Button.DPAD_RIGHT) && transform.position.y < limit)
            {
                controls[1] = vitesse;
            }
            else if (j.GetButton(Joycon.Button.DPAD_LEFT) && transform.position.y > -limit)
            {
                controls[1] = -vitesse;
            }
            else
            {
                controls[1] = 0;
            }

            transform.position += new Vector3(controls[0], controls[1], controls[2]);
        }
    }

    void DeplacementAutreControl()
    {
        if (mode == (int)Modes.DEPLACEMENT)
        {
            /* Fonctions basiques pour les claviers et manettes classiques. */
            controls[0] = Input.GetAxis("Horizontal") * vitesse;
            controls[2] = Input.GetAxis("Vertical") * vitesse;

            /* Ces boutons pour monter ou descendre */
            if (Input.GetButton("Jump") && transform.position.y < limit)
            {
                controls[1] = vitesse;
            }
            else if (Input.GetButton("Fire1") && transform.position.y > -limit)
            {
                controls[1] = -vitesse;
            }
            else
            {
                controls[1] = 0;
            }

            transform.position += new Vector3(controls[0], controls[1], controls[2]);
        }
    }

    /* On passe d'un mode à l'autre par simple pression d'un bouton. */
    void ChangementMode()
    {
        if (selection != null && j.GetButtonUp(Joycon.Button.SR))
        {
            if (mode == (int)Modes.DEPLACEMENT)
            {
                mode = (int)Modes.OBSERVATION;
                selection.AddComponent<Observation>(); // On ajoute ce script à l'objet sélectionné.
            }
            else if (mode == (int)Modes.OBSERVATION && selection.tag == "Modifiable")
            {
                mode = (int)Modes.MODIFICATION;
                Destroy(selection.GetComponent<Observation>()); // On retire ce script à l'objet sélectionné.
                selection.GetComponent<ChangeMesh>().setSelected(true);
                /* */
            }
            else
            {
                mode = (int)Modes.DEPLACEMENT;
                Destroy(selection.GetComponent<Observation>()); // On retire ce script à l'objet sélectionné.
                if (selection.GetComponent<ChangeMesh>() != null)
                    selection.GetComponent<ChangeMesh>().setSelected(false);
            }
        }
    }

    void Selectionner(Collider other)
    {
        if (other.tag == "Selectionnable" || other.tag == "Modifiable")
        {
            if (mode == (int)Modes.DEPLACEMENT)
                if (Input.GetButton("Fire3") || (j != null && j.GetButton(Joycon.Button.DPAD_DOWN)))
                {
                    selection = other.gameObject;
                    selection.GetComponent<Rigidbody>().useGravity = false;
                }
        }
    }

    void Lacher()
    {
        if (selection != null)
        {
            selection.transform.position = transform.position;

            if (mode == (int)Modes.DEPLACEMENT && (Input.GetButton("Fire2") || (j != null && j.GetButton(Joycon.Button.DPAD_UP))))
            {
                selection.GetComponent<Rigidbody>().useGravity = true;
                selection = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.tag == "Selectionnable" || other.tag == "Modifiable")
        {
            couleurBase = other.gameObject.GetComponent<Renderer>().material.color;
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void OnTriggerStay(Collider other)
    {
        Selectionner(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Selectionnable" || other.tag == "Modifiable")
            other.gameObject.GetComponent<Renderer>().material.color = couleurBase;
    }
}
