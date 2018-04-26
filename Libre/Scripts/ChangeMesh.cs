using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChangeMesh : MonoBehaviour
{
    private bool selected;
    Mesh mesh;
    Vector3[] verts;
    public List<GameObject> refVerts = new List<GameObject>();

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh; // Permet de récupérer le maillage associé à l'objet.
        verts = mesh.vertices; // On récupère l'ensemble des sommets du maillage de base.
        selected = false;
        int i = 0;

        /* On fusionne tous les sommets superposés et on crée des objets enfants "Vertex".*/
        foreach (Vector3 vert in verts)
        {
            AddVertex(vert, i);
            i++;
        }
    }

    void Update()
    {
        GameObject handle;
        /* On parcourt tous les objets enfants "Vertex" */
        for (int child = 0; child < transform.childCount; child++)
        {
            handle = transform.GetChild(child).gameObject; // On récupère l'enfant à la position courante.

            /* Pour chaque sommet associé à un objet Vertex, on met à jour sa position dans le tableau de sommets. */
            foreach (int pos in handle.GetComponent<Vertex>().vertices)
            {
                verts[pos] = handle.transform.localPosition;
            }
        }

        mesh.vertices = verts; // On associe les nouvelles positions des sommets à notre gameObject.
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh; // La forme du collider est mise à jour.
    }

    /* 
     * Permet de fusionner les sommets qui devraient être les mêmes.
     * vert est le sommet à fusionner.
     * id est la position du sommet dans le tableau. 
     */
    private void AddVertex(Vector3 vert, int id)
    {
        GameObject g;
        /* On cherche le gameobjet Vertex ayant la même position que le sommet "vert". */
        foreach (GameObject v in refVerts)
        {
            /* Si le sommet et le Vertex correspondent, on donne la référence du sommet au Vertex. */
            if (vert == v.transform.localPosition)
            {
                v.GetComponent<Vertex>().Add(id);
                return;
            }
        }
        /* Si aucun Vertex ne correspond, on en crée un nouveau à partir d'une ressource. */
        g = Instantiate((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Mini-Games/Libre/Vertex.prefab", typeof(GameObject)));
        g.transform.position = transform.TransformPoint(vert); //On convertit la position locale vers globale pour pouvoir placer le Vertex.
        g.transform.parent = transform; // Déclarer en tant qu'enfant de gameObject.
        g.GetComponent<Vertex>().Add(id); // On donne la référence du sommet au Vertex.
        refVerts.Add(g); // On ajoute le Vertex créé à refVerts pour retravailler dessus.
    }

    public void setSelected(bool s)
    {
        selected = s;
    }

    public bool isSelected()
    {
        return selected;
    }
}