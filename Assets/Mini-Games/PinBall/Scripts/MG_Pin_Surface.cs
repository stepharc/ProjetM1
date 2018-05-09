using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_Surface : MonoBehaviour {
    private List<Joycon> joycons;
    private Joycon jg, jd;
    private bool bug;

    public Joycon getLeftJoycon()
    {
        return jg;
    }

    public Joycon getRightJoycon()
    {
        return jd;
    }

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

    // Use this for initialization
    void Start () {
        joycons = JoyconManager.Instance.j;
        bug = false;
        if (joycons.Count == 2)
        {
            if (initJoycons())
            {
                Debug.Log("init");
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

        }
	}
}
