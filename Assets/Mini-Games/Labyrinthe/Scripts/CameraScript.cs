using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject ball;
    public Vector3 offset;

    /* Déplace la caméra en fonction de l'objet ball avec un offset particulier */
    void Update()
    {
        transform.position = new Vector3(ball.transform.position.x + offset.x, ball.transform.position.y + offset.y, ball.transform.position.z + offset.z);
    }
}
