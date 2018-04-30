using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_Player : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon jg, jd;
    private enum MODE { ORIENTATION, POINTING }
    private MODE currentMode;
    private MG_PBall_PBall bInstance;
    private bool bug = false;
    private MeshRenderer pRend;

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
                pRend.enabled = true;
                break;
            case MODE.POINTING:
                break;
            default:
                break;
        }
        return (oldMode != currentMode);
    }

    //Déplace l'objet joueur sur la face du Punching Ball.
    private void movePlayerObject()
    {
        gameObject.transform.localPosition = new Vector3(bInstance.transform.position.x, 0, bInstance.transform.position.z);
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
                pRend.enabled = false;
                break;
            default:
                break;
        }
        return (oldMode != currentMode);
    }

    // Use this for initialization
    void Start () {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count == 2)
        {
            if (initJoycons())
            {
                //On cache l'objet sans désactiver ses composants (et surtout ce script)
                pRend = GetComponent<MeshRenderer>();
                currentMode = MODE.ORIENTATION;
                pRend.enabled = false;
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
            if(currentMode == MODE.POINTING)
            {
                movePlayerObject();
            }
        }
	}
}
