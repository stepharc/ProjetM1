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
    private bool stickPushed;
    private Color couleurBase;
    private Color couleurSommet;
    private GameObject cam;
    public GameObject selection; // L'objet selectionné par le curseur.
    public GameObject sommet; // Le sommet observé en mode Modification.

    [Range(1, 3)]
    public int mode;
    public float[] stick;
    public float vitesse;
    public float limit; // Une limite sur l'axe y pour la hauteur du curseur.
    public int jc_ind = 0;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
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
            ChoixSommetJoycon();
        }
        else
        {
            DeplacementAutreControle();
            ChoixSommetAutreControle();
        }
        ChangementMode();
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

    void ChoixSommetJoycon()
    {
        if (mode == (int)Modes.MODIFICATION)
        {
            /* On peut changer de sommet si aucun n'a été sélectionné */
            if (!sommet.GetComponent<Vertex>().isSelected())
            {
                stick = j.GetStick();

                /* On doit relâcher le stick entre deux changements de sommets. */
                if (stick[1] < 0.5 && stick[1] > -0.5)
                {
                    stickPushed = false;
                }

                /* Si le stick est incliné */
                if (stick[1] < -0.5 && !stickPushed)
                {
                    sommet.GetComponent<MeshRenderer>().material.color = couleurSommet; // Le sommet précédent redevient de couleur normale.
                    sommet = selection.GetComponent<ChangeMesh>().getNextVertex(); // On récupère le sommet suivant.
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow; // On change la couleur du sommet pour le mettre en évidence.
                    stickPushed = true;
                }
                else if (stick[1] > 0.5 && !stickPushed)
                {
                    sommet.GetComponent<MeshRenderer>().material.color = couleurSommet;
                    sommet = selection.GetComponent<ChangeMesh>().getPreviousVertex(); // On récupère le sommet précédent.
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    stickPushed = true;
                }

                if (j.GetButtonDown(Joycon.Button.DPAD_DOWN)) // On valide le sommet voulu.
                {
                    sommet.GetComponent<Vertex>().setSelected(true);
                    sommet.GetComponent<MeshRenderer>().material.color = Color.white; // On change à nouveau la couleur.
                }
            }
            else
            {
                if (j.GetButtonDown(Joycon.Button.DPAD_UP)) // On déselectionne le sommet.
                {
                    sommet.GetComponent<Vertex>().setSelected(false);
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
    }

    void DeplacementAutreControle()
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

    void ChoixSommetAutreControle()
    {
        if (mode == (int)Modes.MODIFICATION)
        {
            if (!sommet.GetComponent<Vertex>().isSelected())
            {
                float direction = Input.GetAxis("Horizontal");
                /* On doit relâcher le stick entre deux changements de sommets. */
                if (direction < 0.5 && direction > -0.5)
                {
                    stickPushed = false;
                }

                /* Si le stick est incliné */
                if (direction < -0.5 && !stickPushed)
                {
                    sommet.GetComponent<MeshRenderer>().material.color = couleurSommet; // Le sommet précédent redevient de couleur normale.
                    sommet = selection.GetComponent<ChangeMesh>().getNextVertex(); // On récupère le sommet suivant.
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow; // On change la couleur du sommet pour le mettre en évidence.
                    stickPushed = true;
                }
                else if (direction > 0.5 && !stickPushed)
                {
                    sommet.GetComponent<MeshRenderer>().material.color = couleurSommet;
                    sommet = selection.GetComponent<ChangeMesh>().getPreviousVertex(); // On récupère le sommet précédent.
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    stickPushed = true;
                }

                if (Input.GetButtonDown("Fire2")) // On valide le sommet voulu.
                {
                    sommet.GetComponent<Vertex>().setSelected(true);
                    sommet.GetComponent<MeshRenderer>().material.color = Color.white; // On change à nouveau la couleur.
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire3")) // On déselectionne le sommet.
                {
                    sommet.GetComponent<Vertex>().setSelected(false);
                    sommet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }

    }

    /* On passe d'un mode à l'autre par simple pression d'un bouton. */
    void ChangementMode()
    {
        if (selection != null && (Input.GetButtonUp("Mode") || (j != null && j.GetButtonUp(Joycon.Button.SR))))
        {
            if (mode == (int)Modes.DEPLACEMENT)
            {
                mode = (int)Modes.OBSERVATION;
                selection.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                selection.AddComponent<Observation>(); // On ajoute ce script à l'objet sélectionné.

                cam.GetComponent<CameraScript>().offset = new Vector3(0, 0, -3); // On rapproche la caméra de l'objet.
            }
            else if (mode == (int)Modes.OBSERVATION && selection.tag == "Modifiable")
            {
                mode = (int)Modes.MODIFICATION;
                Destroy(selection.GetComponent<Observation>()); // On retire ce script de l'objet sélectionné.
                selection.GetComponent<ChangeMesh>().setSelected(true);
                sommet = selection.GetComponent<ChangeMesh>().getCurrentVertex(); // On récupère le premier sommet de la liste.
                couleurSommet = sommet.GetComponent<MeshRenderer>().material.color; // On récupère sa couleur de base.
                sommet.GetComponent<MeshRenderer>().material.color = Color.yellow; // On attribue une couleur de sélection.
            }
            else
            {
                mode = (int)Modes.DEPLACEMENT;
                cam.GetComponent<CameraScript>().offset = new Vector3(0, 2, -5);// On remet la caméra à sa place.
                Destroy(selection.GetComponent<Observation>()); // On retire ce script à l'objet sélectionné.
                if (selection.GetComponent<ChangeMesh>() != null)
                {
                    selection.GetComponent<ChangeMesh>().setSelected(false); // On déselectionne le gameObject.
                    sommet.GetComponent<MeshRenderer>().material.color = couleurSommet; // On remet la couleur de base au sommet.
                    sommet.GetComponent<Vertex>().setSelected(false); // On déselectionne le sommet si sélectionné.
                }
            }
        }
    }

    void Selectionner(Collider other)
    {
        if (other.tag == "Selectionnable" || other.tag == "Modifiable")
        {
            if (mode == (int)Modes.DEPLACEMENT)
                if (Input.GetButton("Fire2") || (j != null && j.GetButton(Joycon.Button.DPAD_DOWN)))
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

            if (mode == (int)Modes.DEPLACEMENT && (Input.GetButton("Fire3") || (j != null && j.GetButton(Joycon.Button.DPAD_UP))))
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
