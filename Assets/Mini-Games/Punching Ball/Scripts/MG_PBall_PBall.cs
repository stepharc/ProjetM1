using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_PBall : MonoBehaviour {
    private string pMode;
    private Joycon jg;

    public void setLeftJoycon(Joycon j)
    {
        jg = j;
    }

    //Récupère le mode dans lequel le joueur se trouve actuellement.
    public void setPlayerMode(string s)
    {
        pMode = s;
    }
    
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
        float orientation;
        switch (pMode)
        {
            //L'utilisateur choisit la face du Punching Ball à taper.
            case "ORIENTATION":
                orientation = jg.GetGyro().z;
                gameObject.transform.Rotate(new Vector3(0, orientation, 0));
                break;
            //L'utilisateur sélectionne la zone du Punching Ball qu'il va taper (cf. script Player)
            case "POINTING":
                break;
            default:
                Debug.Log("Action inconnue (pMode = " + pMode + ")");
                break;
        }
	}
}
