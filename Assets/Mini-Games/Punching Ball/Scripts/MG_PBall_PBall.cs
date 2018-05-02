using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_PBall : MonoBehaviour {
    private string pMode;
    private Joycon jg;
    private Vector3 lastContactPoint;

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
    void Update()
    {
        switch (pMode)
        {
            //L'utilisateur choisit la face du Punching Ball à taper.
            case "ORIENTATION":
                break;
            //L'utilisateur sélectionne la zone du Punching Ball qu'il va taper (cf. script Player)
            case "POINTING":
                break;
            //L'utilisateur peut laisser une marque sur le Punching Ball.
            case "PUNCHING":
                break;
            default:
                Debug.Log("Action inconnue (pMode = " + pMode + ")");
                break;
        }
    }

    //Si l'utilisateur est en mode POINTING, on enregistre le dernier point de collision répertorié
    //entre la balle et le joueur, pour le mode PUNCHING.
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (pMode.CompareTo("POINTING") == 0)
        {
            lastContactPoint = collisionInfo.contacts[0].point;
            Debug.Log(lastContactPoint);
        }
    }
}
