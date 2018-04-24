using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex : MonoBehaviour
{
    /* La liste des références des sommets communs. */
    public List<int> vertices = new List<int>();

    public void Add(int id)
    {
        vertices.Add(id);
    }
}
