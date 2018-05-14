using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramasser : MonoBehaviour
{
    private GameObject selection; // L'objet avant d'être ramasser.
    public GameObject main; // Ce qui doit tenir l'objet.
    public GameObject item; // L'objet tenu.

    /* On attribue l'objet sélectionner à la main du personnage. */
    public void Prendre()
    {
        if (item == null && selection != null)
        {
            item = selection;
            item.GetComponent<Rigidbody>().useGravity = false; /* L'objet tenu n'est plus sous l'influence de la gravité. */
            foreach (Collider col in item.GetComponents<Collider>()) /* On desactive les colliders */
            {
                col.enabled = false;
            }
            item.transform.position = main.transform.position; /* On déplace l'item dans la main. */
            item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; /* On bloque tout mouvement provoquer par le rigidbody. */
            item.transform.parent = main.transform; /* On définit la main comme le parent de l'item. */
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    /* On inverse les valeurs prises dans la fonction Prendre() */
    public void Lacher()
    {
        if (item != null)
        {
            foreach (Collider col in item.GetComponents<Collider>())
            {
                col.enabled = true;
            }
            item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.parent = null;
            item = null;
        }
    }

    /* Lorsqu'on entre en contact avec l'objet, on met un focus dessus. */
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            selection = other.gameObject;
        }
    }

    /* Si perd le contact avec l'objet, on enlève le focus. */
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            selection = null;
        }
    }
}
