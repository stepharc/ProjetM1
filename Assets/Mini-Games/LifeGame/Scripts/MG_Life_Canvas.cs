using UnityEngine;

public class MG_Life_Canvas : MonoBehaviour {
    private int nbGen, minGen, maxGen;

    public int getNbGen()
    {
        return nbGen;
    }

    public void setNbGen(int n)
    {
        if(n >= minGen && n <= maxGen)
        {
            nbGen = n;
        }
    }
    
    void Start()
    {
        nbGen = minGen = 0;
        maxGen = 8;
    }

    void Update()
    {
        gameObject.transform.GetChild(1).GetComponent<MG_Life_Text>().setText("Population (copies) : " + GameObject.FindGameObjectsWithTag("Copy").Length);
        gameObject.transform.GetChild(0).GetComponent<MG_Life_Text>().setText("Generations : " + nbGen);
    }
}
