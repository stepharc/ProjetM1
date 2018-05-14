using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viseur : MonoBehaviour
{
    private Movable current; // On récupère le script de l'objet visé.
    private List<Joycon> joycons;
    private Joycon j;
    private bool aim; // Bouton de selection pressé ou non.

    public float HauteurMax;
    public float HauteurMin;
    public float vitesseH = 1;
    public float vitesseV = 1;
    public int jc_ind = 0;
    // Use this for initialization
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            j = joycons[jc_ind];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Mouvements();
        Selection();
    }

    private void Mouvements()
    {
        float h, v;
        if (joycons.Count > 0)
        {
            if (j.isLeft)
            {
                h = -j.GetStick()[1] * vitesseH;
                v = j.GetStick()[0] * vitesseV;
                aim = j.GetButtonDown(Joycon.Button.DPAD_DOWN);
            }
            else
            {
                h = j.GetStick()[1] * vitesseH;
                v = -j.GetStick()[0] * vitesseV;
                aim = j.GetButtonDown(Joycon.Button.DPAD_UP);
            }
        }
        else
        {
            h = Input.GetAxis("Horizontal") * vitesseH;
            v = Input.GetAxis("Vertical") * vitesseV;
            aim = Input.GetButtonDown("Fire2");
        }

        /* Pour bloquer la position entre les limites en hauteur. */
        if (transform.position.y <= HauteurMin && v < 0)
            v = 0;
        else if (transform.position.y >= HauteurMax && v > 0)
            v = 0;

        transform.rotation = Quaternion.Euler(0, h + transform.rotation.eulerAngles.y, 0);
        transform.position += new Vector3(0, v);
    }

    public void Selection()
    {
        RaycastHit hit;
        /* On crée un rayon invisible qui part du centre de la camera. */
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        if (Physics.Raycast(ray, out hit))
        {
            /* Si le rayon touche un élement avec le tag Movable. */
            if (hit.collider.tag == "Movable")
            {
                if (current != null)
                    current.Aimed(false); // On passe le précédent visé à faux.
                current = hit.collider.gameObject.GetComponent<Movable>(); // On récupère le nouveau visé.
                current.Aimed(true); // On passe l'objet visé à vrai.
                if (aim)
                    current.moved = !current.moved; // Si on appuie sur le bouton on active moved.
            }
            else
            {
                if (current != null)
                    current.Aimed(false); // Si l'objet visé n'a pas le tag Movable, on passe le précédent visé à faux.
            }
        }
    }
}
