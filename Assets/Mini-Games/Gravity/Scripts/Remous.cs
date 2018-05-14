using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Permet de donner un effet de Remous aux surfaces plates*/
public class Remous : MonoBehaviour
{
    public float scale = 0.1f;
    public float speed = 1.0f;

    private Vector3[] baseHeight;

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices; // On stocke la forme de base du l'objet.

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            /* Pour chaque sommet on calcul grâce au Sinus pour rester dans un ensemble de valeurs restreintes. */
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
