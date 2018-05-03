using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PBall_PBall : MonoBehaviour {
    private string pMode;
    private Joycon jg;
    private Vector3 lastContactPoint;
    private Mesh deformingMesh;
    private Vector3[] originalVertices, displacedVertices, vertexVelocities;
    private float force, minForce, maxForce;

    public void setLeftJoycon(Joycon j)
    {
        jg = j;
    }

    //Récupère le mode dans lequel le joueur se trouve actuellement.
    public void setPlayerMode(string s)
    {
        pMode = s;
    }
    
	// Use this for initialization
	void Start () {
        //Empêche tout mouvement non souhaité avec le joueur lors d'une collision entre les deux objets.
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        lastContactPoint = new Vector3(0, 0, 0);
        //Initialisation des données pour la force d'impact.
        minForce = 5.0f;
        force = minForce;
        maxForce = 100f;
        //Initialisation des données pour la déformation du Mesh.
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (pMode)
        {
            //L'utilisateur choisit la face du Punching Ball à taper.
            case "ORIENTATION":
                break;
            //L'utilisateur sélectionne la zone du Punching Ball qu'il va taper (cf. script Player)
            case "POINTING":
                break;
            //L'utilisateur peut laisser une marque sur le Punching Ball.
            case "PUNCHING":
                break;
            default:
                Debug.Log("Action inconnue (pMode = " + pMode + ")");
                break;
        }
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

//http://catlikecoding.com/unity/tutorials/mesh-deformation/
