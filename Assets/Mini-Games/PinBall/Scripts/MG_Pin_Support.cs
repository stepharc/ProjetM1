using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_Support : MonoBehaviour {
    private MG_Pin_Surface grandfather;
    private Vector3 spawnPos;
    private Joycon jd;
    private Rigidbody rb;
    private bool rs, us, touch;

    //Déplace le support de bille vers le bas, jusqu'à une certaine limite.
    IEnumerator retractSupport()
    {
        rs = true;
        while (!touch)
        {
            rb.AddForce(-transform.up * 150);
            yield return null;
        }
        touch = false;
        rs = false;
    }

    //Déplace le support de bille vers le haut jusqu'à sa position de départ.
    IEnumerator unleashSupport()
    {
        us = true;
        while(gameObject.transform.position.y < spawnPos.y)
        {
            //On utilise cette méthode au lieu de Transform.Translate pour éviter un problème de
            //collision avec la bille (cette dernière traverserait le support pendant que l'objet
            //bouge vers le haut).
            rb.AddForce(transform.up * 180);
            yield return null;
        }
        us = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Collision entre le support et le bas de la surface de jeu pendant le mouvement
        //vers le bas du support : on stoppe ce déplacement en changeant le drapeau utilisé
        //dans la co-routine retractSupport.
        if((rs) && (collision.gameObject.name.CompareTo(grandfather.name) == 0))
        {
            touch = true;
        }
    }

    // Use this for initialization
    void Start () {
        MG_Pin_Arms father = gameObject.transform.parent.GetComponent<MG_Pin_Arms>();
        grandfather = father.getFather();
        spawnPos = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        //Evite les rotations de l'objet indésirables.
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rs = us = touch = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(jd != null)
        {
            //Bouton A (Joycon Droit)
            if (jd.GetButtonDown(Joycon.Button.DPAD_RIGHT))
            {
                if ((!rs) && (!us))
                {
                    StartCoroutine("retractSupport");
                }
            }
            if (jd.GetButtonUp(Joycon.Button.DPAD_RIGHT))
            {
                if (!us)
                {
                    StopCoroutine("retractSupport");
                    if (rs) rs = false;
                    StartCoroutine("unleashSupport");
                }
            }
            if(gameObject.transform.position.y > spawnPos.y)
            {
                //On replace le palet exactement à sa position initiale (pour éviter que la bille ne se trouve
                //en dessous du palet lors de sa réapparition suite à une défaite)
                gameObject.transform.position = spawnPos;
            }
        }
        else
        {
            jd = grandfather.getRightJoycon();
        }
	}
}
