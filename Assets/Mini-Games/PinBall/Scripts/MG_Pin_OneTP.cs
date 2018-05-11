using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Pin_OneTP : MonoBehaviour {
    private MG_Pin_TPs father;

    //Pendant un certain délai (2 secondes), les téléporteurs sont désactivés, afin d'éviter les téléportations successives.
    IEnumerator disableTPs()
    {
        father.setWarpValue(true);
        yield return new WaitForSeconds(2f);
        father.setWarpValue(false);
    }

    private void OnTriggerEnter(Collider c)
    {
        bool b = father.getWarpValue();
        //Si la bille entre en contact avec la bôîte de collision du téléporteur et que ces derniers ne sont pas désactivés.
        if ((c.name.CompareTo("Bille") == 0) && (!b))
        {
            StartCoroutine("disableTPs");
            Color[] cs = father.getColors();
            int index = father.getIndexByColor(gameObject.GetComponent<Renderer>().material.color);
            List<Transform> l = father.getAmputatedList(gameObject.transform, index);
            if(l.Count != 0)
            {
                //On choisit un téléporteur au hasard parmi ceux éligibles.
                int r = Random.Range(0, l.Count - 1);
                Transform t = l[r];
                //La bille est déplacée sous le téléporteur élu.
                c.gameObject.transform.position = new Vector3(t.position.x, t.position.y - (c.gameObject.transform.lossyScale.y / 2), t.position.z);
            }
            //Le téléporteur sélectionne une nouvelle couleur aléatoirement parmi ceux proposés.
            int rand = Random.Range(0, cs.Length);
            gameObject.GetComponent<Renderer>().material.color = cs[rand];
            father.setElementInList(gameObject.transform, rand);
        }
    }

    // Use this for initialization
    void Start () {
        father = gameObject.transform.parent.GetComponent<MG_Pin_TPs>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
