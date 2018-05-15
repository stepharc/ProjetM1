using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Life_Cube : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon jg, jd;
    private bool bug, copy, changeMat;
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
        if (changeMat) changeMat = false;
        //Crée une copie de l'objet contrôlé  ...
        GameObject g = GameObject.Instantiate(gameObject);
        g.SetActive(false);
        g.tag = "Copy";
        g.name = "InitialCopy";
        //... sur laquelle on remplace ce script par celui réservé pour les clones ...
        Destroy(g.GetComponent<MG_Life_Cube>());
        g.AddComponent<MG_Life_Clone>();
        g.GetComponent<MG_Life_Clone>().setGenLeft(10);
        //... et en supprimant tous les enfants de l'objet original.
        foreach (Transform child in g.transform)
        {
            Destroy(child.gameObject);
        }
        //On réactive le clone pour qu'il puisse commencer à produire des copies.
        g.SetActive(true);
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

    //Produit la translation à une vitesse speed OU l'orientation à une vitesse speed * 4
    //de l'objet contrôlé par le joueur selon l'orientation des sticks des Joycons toutes passées en paramètre.
    private void stickMovements(float leftX, float leftY, float rightX, float rightY, float speed)
    {
        //Stick droit : Haut / Bas / Rotation Gauche / Rotation Droit
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
        if (rightX > 0)
        {
            gameObject.transform.Rotate(Vector3.up * (speed * 4) * Time.deltaTime);
        }
        else
        {
            if (rightX < 0)
            {
                gameObject.transform.Rotate(Vector3.down * (speed * 4) * Time.deltaTime);
            }
        }
        //Stick gauche : Avant / Arrière / Gauche / Droite
        if (leftX > 0)
        {
            gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            if(leftX < 0)
            {
                gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
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
                changeMat = true;
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
            float lsx = jg.GetStick()[0], lsy = jg.GetStick()[1], rsx = jd.GetStick()[0], rsy = jd.GetStick()[1];
            stickMovements(lsx, lsy, rsx, rsy, 25f);
            //Bouton X pressé et il est encore possible de changer de matériau.
            if (jd.GetButtonDown(Joycon.Button.DPAD_UP) && (changeMat))
            {
                switchMaterial();
            }
            //Bouton A pressé et possibilité de créer un clone.
            if (jd.GetButtonDown(Joycon.Button.DPAD_RIGHT) && (!copy))
            {
                StartCoroutine("createClone");
            }
            //Bouton B pressé. On supprime tout les clones générés et on peut de nouveau changer de matériau.
            if (jd.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("Copy");
                foreach(GameObject go in objects)
                {
                    Destroy(go);
                }
                if (!changeMat) changeMat = true;
            }
        }
	}
}
