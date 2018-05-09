using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_Arms : MonoBehaviour {
    private Joycon jg, jd;
    private Transform la, ra;
    private MG_Pin_Surface father;
    private bool mla, rla, mra, rra;

    public void setLeftArm(Transform t)
    {
        la = t;
    }

    //Effectue le mouvement du bras gauche vers le haut jusqu'à une valeur maximale.
    IEnumerator moveLeftArm()
    {
        mla = true;
        float max = 110f, currentRotation = la.rotation.eulerAngles.y;
        while(currentRotation > max)
        {
            la.Rotate(Vector3.down, 300f * Time.deltaTime);
            currentRotation = la.rotation.eulerAngles.y;
            yield return null;
        }
        mla = false;
    }

    //Effectue le mouvement du bras gauche vers le bas jusqu'à sa position d'origine.
    IEnumerator returnLeftArm()
    {
        rla = true;
        float min = la.GetComponent<MG_Pin_LRArm>().getBaseRotation(), currentRotation = la.rotation.eulerAngles.y;
        while (currentRotation < min)
        {
            la.Rotate(Vector3.up, 300f * Time.deltaTime);
            currentRotation = la.rotation.eulerAngles.y;
            yield return null;
        }
        rla = false;
    }

    public void setRightArm(Transform t)
    {
        ra = t;
    }

    IEnumerator moveRightArm()
    {
        mra = true;
        float max = -110f, currentRotation = ra.rotation.eulerAngles.y - 360;
        while (currentRotation < max)
        {
            ra.Rotate(Vector3.up, 300f * Time.deltaTime);
            //eulerAngles ne gère pas les valeurs négatives. Or, nous sommes actuellement en train de manipuler
            //une telle valeur : il faut donc effectuer cette soustraction pour obtenir la valeur voulue.
            currentRotation = ra.rotation.eulerAngles.y - 360;
            yield return null;
        }
        mra = false;
    }

    IEnumerator returnRightArm()
    {
        rra = true;
        float min = ra.GetComponent<MG_Pin_LRArm>().getBaseRotation() - 360, currentRotation = ra.rotation.eulerAngles.y;
        while (currentRotation > min)
        {
            ra.Rotate(Vector3.down, 300f * Time.deltaTime);
            currentRotation = ra.rotation.eulerAngles.y - 360;
            yield return null;
        }
        rra = false;
    }

    // Use this for initialization
    void Start () {
        father = gameObject.transform.parent.GetComponent<MG_Pin_Surface>();
        mla = rla = mra = rra = false;
	}
	
	// Update is called once per frame
	void Update () {
        if ((jg != null) && (jd != null))
        {
            //Bouton ZL pressé et bras gauche initialisé.
            if ((jg.GetButtonDown(Joycon.Button.SHOULDER_2)) && (la != null))
            {
                //On s'assure que la co-routine moveLeftArm ne soit appelée qu'une seule fois.
                if (!mla)
                {
                    StopCoroutine("returnLeftArm");
                    if (rla) rla = false;
                    StartCoroutine("moveLeftArm");
                }
            }
            //Bouton ZR pressé et bras droit initialisé : on stoppe le retour du bras droit à
            //sa position initiale (même si c'est en cours) et on fait bouger le bras droit vers le haut.
            if ((jd.GetButtonDown(Joycon.Button.SHOULDER_2)) && (ra != null))
            {
                if (!mra)
                {
                    StopCoroutine("returnRightArm");
                    if (rra) rra = false;
                    StartCoroutine("moveRightArm");
                }
            }
            //Bouton ZL relâché et bras gauche initialisé : on stoppe le mouvement du bras gauche vers
            //le haut (même si c'est en cours) et on entame le retour du bras gauche vers sa position initiale.
            if ((jg.GetButtonUp(Joycon.Button.SHOULDER_2)) && (la != null))
            {
                if (!rla)
                {
                    StopCoroutine("moveLeftArm");
                    if (mla) mla = false;
                    StartCoroutine("returnLeftArm");
                }
            }
            //Bouton ZR relâché et bras droit initialisé.
            if ((jd.GetButtonUp(Joycon.Button.SHOULDER_2)) && (ra != null))
            {
                if (!rra)
                {
                    StopCoroutine("moveRightArm");
                    //Si la co-routine ci-dessus a été stoppée prématurément, on n'oublie 
                    //pas de préciser que cette dernière n'est plus en cours d'exécution.
                    if (mra) mra = false;
                    StartCoroutine("returnRightArm");
                }
            }
        }
        else
        {
            //On initialise ici et pas dans la fonction Start car celle du parent ne s'active
            //qu'une fois tous ses enfants initialisés.
            jg = father.getLeftJoycon();
            jd = father.getRightJoycon();
        }
	}
}
