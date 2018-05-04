using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject focus; // L'objet qui a le focus.
    public Vector3 offset; // La distance entre la caméra et l'objet qui a le focus.

    /* Déplace la caméra en fonction de l'objet ball avec un offset particulier */
    void Update()
    {
        transform.position = new Vector3(focus.transform.position.x + offset.x, focus.transform.position.y + offset.y, focus.transform.position.z + offset.z);
    }
}
