using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Life_Clone : MonoBehaviour {
    private int remaining = 6, nbClones, generationsLeft;
    private GameObject[] neighbors = new GameObject[6];
    public enum ORIENTATION { UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD};

    public void setGenLeft(int g)
    {
        generationsLeft = g;
    }

    //Retourne la position du père de l'objet courant par rapport à ce dernier.
    private ORIENTATION toClonePerspective(ORIENTATION o)
    {
        ORIENTATION res = ORIENTATION.UP;
        switch (o)
        {
            case ORIENTATION.UP:
                res = ORIENTATION.DOWN;
                break;
            case ORIENTATION.DOWN:
                res = ORIENTATION.UP;
                break;
            case ORIENTATION.LEFT:
                res = ORIENTATION.RIGHT;
                break;
            case ORIENTATION.RIGHT:
                res = ORIENTATION.LEFT;
                break;
            case ORIENTATION.FORWARD:
                res = ORIENTATION.BACKWARD;
                break;
            case ORIENTATION.BACKWARD:
                res = ORIENTATION.FORWARD;
                break;
        }
        return res;
    }

    //Place le père de l'objet courant à sa bonne place dans le tableau
    //neighbors selon sa position par rapport à son fils.
    public void setInitialNeighbor(GameObject g, ORIENTATION o)
    {
        ORIENTATION n = toClonePerspective(o);
        neighbors[(int) n] = g;
        remaining--;
    }

    //Cherche la <place> ème case vide du tableau neighbors et retourne l'orientation
    //correspondant à l'indice de la case trouvée.
    private ORIENTATION setNeighbor(int place)
    {
        int distance = place + 1;
        for(int i = 0; i < neighbors.Length; i++)
        {
            if(neighbors[i] == null)
            {
                distance--;
                if(distance == 0)
                {
                    return (ORIENTATION) i;
                }
            }
        }
        return (ORIENTATION) 0;
    }

    //Retourne la position de la future copie à créer, selon l'orientation passée en paramètre,
    //de telle sorte qu'elle se trouve "collée" à la face de l'objet courant correspondant.
    //(Exemple : ORIENTATION.UP -> Sur la face supérieure de l'objet courant)
    private Vector3 getPositionNewCopy(ORIENTATION o)
    {
        Vector3 res = gameObject.transform.position, dim = gameObject.transform.lossyScale;
        switch (o)
        {
            case ORIENTATION.UP:
                res = new Vector3(res.x, res.y + dim.y, res.z);
                break;
            case ORIENTATION.DOWN:
                res = new Vector3(res.x, res.y - dim.y, res.z);
                break;
            case ORIENTATION.LEFT:
                res = new Vector3(res.x - dim.x, res.y, res.z);
                break;
            case ORIENTATION.RIGHT:
                res = new Vector3(res.x + dim.x, res.y, res.z);
                break;
            case ORIENTATION.FORWARD:
                res = new Vector3(res.x, res.y, res.z + dim.z);
                break;
            case ORIENTATION.BACKWARD:
                res = new Vector3(res.x, res.y, res.z - dim.z);
                break;
        }
        return res;
    }

    IEnumerator generateCopies()
    {
        int randPlace;
        Vector3 newPos;
        //Pour chaque clone à créer :
        for(int i = 1; i <= nbClones; i++)
        {
            //On choisit une place parmi ceux libres.
            randPlace = Random.Range(0, remaining - 1);
            ORIENTATION o = setNeighbor(randPlace);
            newPos = getPositionNewCopy(o);
            GameObject g = GameObject.Instantiate(gameObject, newPos, gameObject.transform.rotation);
            g.SetActive(false);
            g.name = g.tag + "-" + generationsLeft + "-" + i;
            Destroy(g.GetComponent<MG_Life_Cube>());
            foreach (Transform child in g.transform)
            {
                Destroy(child.gameObject);
            }
            neighbors[(int)o] = g;
            remaining--;
            g.GetComponent<MG_Life_Clone>().setInitialNeighbor(gameObject, o);
            g.GetComponent<MG_Life_Clone>().setGenLeft(generationsLeft - 1);
            g.hideFlags = HideFlags.HideInHierarchy;
            g.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
        System.Array.Clear(neighbors, 0, neighbors.Length);
    }

    //Cette méthode s'exécute automatiquement lorsqu'un clone désactivé redevient 
    //actif via la méthode SetActive(true). On l'utilise ici pour générer un nombre
    //aléatoire de voisins de cet objet à des positions aléatoires.
    private void OnEnable()
    {
        //S'il ne s'agit pas d'un clone issu de la dernière génération, on génère nbClones
        //voisins.
        if (generationsLeft != 0)
        {
            nbClones = Random.Range(1, remaining);
            StartCoroutine("generateCopies");
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
