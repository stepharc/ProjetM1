using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_PBall : MonoBehaviour {
    private string pMode;
    private Vector3 lastContactPoint;
    private Mesh deformingMesh;
    private float force, damping, springForce;
    private Vector3[] originalVertices, displacedVertices, vertexVelocities;

    public void setForce(float f)
    {
        force = f;
    }

    //Récupère le mode dans lequel le joueur se trouve actuellement.
    public void setPlayerMode(string s)
    {
        pMode = s;
    }

    //Merci à la source http://catlikecoding.com/unity/tutorials/mesh-deformation/ pour les 3 fonctions ci-après.

    //Applique une force de déformation sur le vertex i
    private void AddForceToVertex(int i)
    {
        //Convertit la force en vélocité. Plus le vertex est loin du point de contact, plus la force s'atténue (effet de propagation)
        Vector3 pointToVertex = displacedVertices[i] - lastContactPoint;
        //Pour calculer l'atténuation de force, application de la loi en carré inverse (https://fr.wikipedia.org/wiki/Loi_en_carr%C3%A9_inverse)
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        //Applique à la vélocité du vertex i une direction, que le vertex va prendre lorsqu'il se déformera.
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    public void AddDeformingForce()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceToVertex(i);
        }
    }

    //Déforme le vertex i, puis revient au bout d'un certain temps à son aspect normal.
    private void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        //Déformation ...
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        velocity -= displacement * springForce * Time.deltaTime;
        //On diminue petit à petit la vélocité du vertex i
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * Time.deltaTime;
    }

    // Use this for initialization
    void Start () {
        //Empêche tout mouvement non souhaité avec le joueur lors d'une collision entre les deux objets.
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        lastContactPoint = new Vector3(0, 0, 0);
        //Initialisation des données pour la déformation du Mesh.
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        //Valeur faible : l'impact aura un effet rebondissant. Valeur élevée : effet mou.
        damping = 4f;
        //Ajusteur de vélocité : plus cette valeur est élevée, plus l'animation d'impact est rapide.
        springForce = 20f;
        force = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    //Si l'utilisateur est en mode POINTING, on enregistre le dernier point de collision répertorié
    //entre la balle et le joueur, pour le mode PUNCHING.
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (pMode.CompareTo("POINTING") == 0)
        {
            lastContactPoint = collisionInfo.contacts[0].point;
        }
    }
}
