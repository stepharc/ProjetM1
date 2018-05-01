using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_Player : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon jg, jd;
    private enum MODE { ORIENTATION, POINTING, PUNCHING }
    private MODE currentMode;
    private MG_PBall_PBall bInstance;
    private bool bug;
    private float timeCounter;
    //Vitesse de rotation (mouvement circulaire)
    private float speed;

    //Récupère l'angle de rotation (pour le mouvement circulaire)
    public float getTimeCounter()
    {
        return timeCounter;
    }

    public string getCurrentMode()
    {
        return currentMode.ToString();
    }

    //Place les joycons connectés dans la variable correspondante selon s'il s'agit du joycon droit ou du gauche.
    //Retourne vrai si les variables jg et jd ont été instanciées, faux sinon.
    private bool initJoycons()
    {
        bool ok = false;
        for(int i = 0; i < joycons.Count; i++)
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
        if((jd != null) && (jg != null)){
            ok = true;
        }
        return ok;
    }

    //Cette fonction essaie de passer au mode suivant. Retourne vrai si l'action a réussi, faux sinon.
    private bool switchMode()
    {
        MODE oldMode = currentMode;
        switch (currentMode)
        {
            case MODE.ORIENTATION:
                currentMode = MODE.POINTING;
                break;
            case MODE.POINTING:
                currentMode = MODE.PUNCHING;
                break;
            case MODE.PUNCHING:
                break;
            default:
                break;
        }
        return (oldMode != currentMode);
    }

    //Cette fonction essaie de passer au mode précédent. Retourne vrai si l'action a réussi, faux sinon.
    private bool revertMode()
    {
        MODE oldMode = currentMode;
        switch (currentMode)
        {
            case MODE.ORIENTATION:
                break;
            case MODE.POINTING:
                currentMode = MODE.ORIENTATION;
                break;
            case MODE.PUNCHING:
                currentMode = MODE.POINTING;
                break;
            default:
                break;
        }
        return (oldMode != currentMode);
    }

    //Déplace l'objet joueur sur la face du Punching Ball et effectue un mouvement circulaire sur celle-ci.
    private void rotatePlayerObject()
    {
        //Rayon du cercle autour duquel le joueur va effectuer un mouvement circluaire : pour que l'objet soit en surface du punching ball,
        //on doit compter la moitié de la dimension X du punching ball plus celle de l'utilisateur
        float radius = (bInstance.transform.lossyScale.z / 2) + (gameObject.transform.lossyScale.z / 2);
        //On place d'abord l'objet au centre du cercle de rotation.
        gameObject.transform.position = bInstance.GetComponent<Collider>().bounds.center;
        //Puis on réalise le mouvement autour du cercle.
        float newX = gameObject.transform.position.x + (Mathf.Cos(timeCounter) * radius);
        float newZ = gameObject.transform.position.z + (Mathf.Sin(timeCounter) * radius);
        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, newZ);
    }

    //Effectue le déplacement du joueur sur son axe Y (limitée aux dimensions du Punching Ball) lors du mode POINTING selon la touche pressée.
    private void moveYAxisPlayer()
    {
        float moveSpeed = 0.075f, limit, oldY = gameObject.transform.position.y;
        //Bouton "Flèche Haut" enfoncé, on augmente la valeur Y de la position du joueur jusqu'à une certaine limite.
        if (jg.GetButton(Joycon.Button.DPAD_UP))
        {
            limit = bInstance.GetComponent<Collider>().bounds.max.y - (gameObject.transform.lossyScale.y / 2);
            if (oldY < limit)
            {
                gameObject.transform.localPosition += new Vector3(0, moveSpeed, 0);
            }
        }
        //Bouton "Flèche Bas" enfoncé, on diminue la valeur Y de la position du joueur jusqu'à une certaine limite.
        if (jg.GetButton(Joycon.Button.DPAD_DOWN))
        {
            //On fait en sorte que l'objet joueur ne puisse pas aller sous le Punching Ball.
            limit = bInstance.GetComponent<Collider>().bounds.min.y + (gameObject.transform.lossyScale.y / 2);
            if (oldY > limit)
            {
                gameObject.transform.localPosition -= new Vector3(0, moveSpeed, 0);
            }
        }
    }

    //Réalise le mouvement adéquat sur l'objet joueur selon le mode dans lequel on est.
    private void modeAction()
    {
        switch (currentMode)
        {
            //Tant qu'on est en mode orientation, le mouvement circulaire du joueur autour du Punching Ball continue.
            case MODE.ORIENTATION:
                rotatePlayerObject();
                break;
            case MODE.POINTING:
                moveYAxisPlayer();
                break;
            case MODE.PUNCHING:
                break;
            default:
                break;
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
                timeCounter = 0;
                speed = 1;
                currentMode = MODE.ORIENTATION;
                bInstance = GameObject.Find("Punching Ball").GetComponent<MG_PBall_PBall>();
                bInstance.setPlayerMode(currentMode.ToString());
                bInstance.setLeftJoycon(jg);
                gameObject.GetComponent<Renderer>().material.color = Color.black;
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
            //Bouton A pressé, on essaie de passer au mode suivant : si cette action a un effet, on prévient l'objet Punching Ball.
            if (jd.GetButtonDown(Joycon.Button.DPAD_RIGHT))
            {
                if (switchMode())
                {
                    bInstance.setPlayerMode(currentMode.ToString());
                }
            }
            //Bouton B pressé, on essaie de passer au mode précédent : si cette action a un effet, on prévient l'objet Punching Ball.
            if (jd.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                if (revertMode())
                {
                    bInstance.setPlayerMode(currentMode.ToString());
                }
            }
            //Tant qu'on est en mode orientation, on change l'angle de rotation.
            if (currentMode == MODE.ORIENTATION)
            {
                timeCounter += Time.deltaTime * speed;
            }
            modeAction();
        }
	}
}
