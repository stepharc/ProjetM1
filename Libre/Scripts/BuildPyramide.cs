using UnityEngine;
using System.Collections;

public class BuildPyramide : MonoBehaviour
{
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;

        Vector3[] vertices = new Vector3[]
        {
            /* Base de la pyramide */
            new Vector3(-1,-1,1),
            new Vector3(1,-1,1),
            new Vector3(-1,-1,-1),
            new Vector3(1,-1,-1),
            /* Sommet de la pyramide avec une position choisie aléatoirement dans une sphère de rayon 1 */
            Random.insideUnitSphere
        };

        int[] triangles = new int[]
        { 
            /* Chaque triangle du modèle 3D est relié à trois sommets. 
             * Il doivent être inscrit dans le sens horaire des sommets pour s'afficher. */
            0,1,2,
            1,3,2,
            0,2,1,
            1,2,3,
            1,4,0,
            3,4,1,
            2,4,3,
            0,4,2
        };

        // On passe les tableaux à la mesh.
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals(); // Pour l'intéraction avec la lumière.

        gameObject.AddComponent<MeshCollider>().convex = true; // Adapte le collider à la forme.
    }
}
