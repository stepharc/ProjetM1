using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Life_Cube : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon jg, jd;
    private bool bug, copy;
    private string pathGPUI, pathNotGPUI;

    //Place les joycons connectés dans la variable correspondante selon s'il s'agit du joycon droit ou du gauche.
    //Retourne vrai si les variables jg et jd ont été instanciées, faux sinon.
    private bool initJoycons()
    {
        bool ok = false;
        for (int i = 0; i < joycons.Count; i++)
        {
            if (joycons[i].isLeft)
            {
                jg = joycons[i];
            }
            else
            {
                jd = joycons[i];
            }
        }
        if ((jd != null) && (jg != null))
        {
            ok = true;
        }
        return ok;
    }

    IEnumerator createClone()
    {
        copy = true;
        //Crée une copie de l'objet contrôlé ...
        GameObject g = GameObject.Instantiate(gameObject);
        //... sur laquelle on remplace ce script par celui réservé pour les clones ...
        Destroy(g.GetComponent<MG_Life_Cube>());
        g.AddComponent<MG_Life_Clone>();
        //... et en supprimant tous les enfants de l'objet original.
        foreach (Transform child in g.transform)
        {
            Destroy(child.gameObject);
        }
        //Ajout d'un petit délai afin d'éviter le spam d'appui de touche de création de clone.
        yield return new WaitForSeconds(0.75f);
        copy = false;
    }

    //Change le matériau de l'objet contrôlé par le joueur. Si le nom du matériau n'est pas reconnu, rien de notable
    //se passe. A contrario, s'il y a changement, WithGPUI est un matériau avec GPU Instancing ON tandis que 
    //WithoutGPUI n'a pas cette option activée.
    private void switchMaterial()
    {
        Material mat = gameObject.GetComponent<Renderer>().material;
        if(mat.name.Contains("WithGPUI"))
        {
            mat = Resources.Load(pathNotGPUI, typeof(Material)) as Material;
        }
        else
        {
            if(mat.name.Contains("WithoutGPUI"))
            {
                mat = Resources.Load(pathGPUI, typeof(Material)) as Material;
            }
        }
        gameObject.GetComponent<Renderer>().material = mat;
    }

    //Produit la translation à une vitesse speed de l'objet contrôlé par le joueur selon l'orientation des sticks
    //des Joycons toutes (excepté l'axe X du stick gauche, inutile ici) passées en paramètre.
    private void stickMovements(float leftY, float rightX, float rightY, float speed)
    {
        //Stick droit : Haut / Bas / Gauche / Droite
        if (rightY > 0)
        {
            gameObject.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            if (rightY < 0)
            {
                gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
        if(rightX > 0)
        {
            gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            if(rightX < 0)
            {
                gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
        //Stick gauche : Avant / Arrière
        if (leftY > 0)
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            if (leftY < 0)
            {
                gameObject.transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
        }
    }

    // Use this for initialization
    void Start () {
        joycons = JoyconManager.Instance.j;
        bug = false;
        if (joycons.Count == 2)
        {
            if (initJoycons())
            {
                //Chemin vers les matériaux utilisés pour ce mini-jeu. Ces derniers doivent se 
                //trouver sous le dossier Resources qui se trouve lui-même sous le dossier du projet.
                pathGPUI = "materials/withgpui";
                pathNotGPUI = "materials/withoutgpui";
                Material mat = Resources.Load(pathNotGPUI, typeof(Material)) as Material;
                gameObject.GetComponent<Renderer>().material = mat;
                copy = false;
            }
            else
            {
                Debug.Log("Absence du joycon droit ou gauche.");
                bug = true;
            }
        }
        else
        {
            Debug.Log("Pas assez ou trop de joycons détectés. Un joycon droit et un gauche nécessaires.");
            bug = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!bug)
        {
            float lsy = jg.GetStick()[1], rsx = jd.GetStick()[0], rsy = jd.GetStick()[1];
            //Bouton X pressé.
            if (jd.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                switchMaterial();
            }
            stickMovements(lsy, rsx, rsy, 25f);
            //Bouton A pressé et possibilité de créer un clone.
            if (jd.GetButtonDown(Joycon.Button.DPAD_RIGHT) && (!copy))
            {
                StartCoroutine("createClone");
            }
        }
	}
}
