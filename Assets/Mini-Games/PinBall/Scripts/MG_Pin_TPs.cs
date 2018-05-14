using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_TPs : MonoBehaviour {
    private int nbChild, nbColors;
    private Color[] colors;
    private List<List<Transform>> tpByColor;
    private bool warp;

    public Color[] getColors()
    {
        return colors;
    }

    public void setWarpValue(bool b)
    {
        warp = b;
    }

    public bool getWarpValue()
    {
        return warp;
    }

    private Color setRandomColor()
    {
        float r, g, b;
        r = Random.Range(0f, 1f);
        g = Random.Range(0f, 1f);
        b = Random.Range(0f, 1f);
        return new Color(r, g, b);
    }

    //Retourne la position de la couleur c dans le tableau colors. -1 si cette couleur n'est pas dans le tableau.
    public int getIndexByColor(Color c)
    {
        for(int i = 0; i < colors.Length; i++)
        {
            if(colors[i] == c)
            {
                return i;
            }
        }
        return -1;
    }

    //Retire l'élément Transform passé en paramètre de la liste i avant de la retourner.
    public List<Transform> getAmputatedList(Transform t, int i)
    {
        tpByColor[i].Remove(t);
        return tpByColor[i];
    }

    public void setElementInList(Transform t, int i)
    {
        tpByColor[i].Add(t);
    }

    // Use this for initialization
    void Start () {
        int rand;
        warp = false;
        nbColors = 3;
        tpByColor = new List<List<Transform>>();
        colors = new Color[nbColors];
        for(int i = 0; i < nbColors; i++)
        {
            colors[i] = setRandomColor();
            tpByColor.Add(new List<Transform>());
        }
        nbChild = gameObject.transform.childCount;
        //Pour chaque enfant (= téléporteur), on lui sélectionne une couleur de départ parmi 
        //les nbColors couleurs aléatoires générées, puis on le place correctement dans la bonne liste
        //(Exemple : si reçu couleur d'indice 1 dans le tableau colors, le téléporteur sera placé dans la liste
        //d'indice 1 dans la liste tpByColor).
		for(int i = 0; i < nbChild; i++)
        {
            Transform t = gameObject.transform.GetChild(i);
            rand = Random.Range(0, colors.Length);
            t.GetComponent<Renderer>().material.color = colors[rand];
            setElementInList(t, rand);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
