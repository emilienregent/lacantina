using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.2f;
    bool childInRange = false;


    // Update is called once per frame
    void Update()
    {
        // Déplacement du joueur
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Move(horizontal, vertical);

        if (childInRange == true)
        {
            Action1();
            Action2();
            Action3();
            Action4();
        }

    }

    // Déplacements
    void Move(float horizontal, float vertical)
    {
        // Création d'un nouveau vecteur de déplacement
        Vector3 move = new Vector3();

        // Récupération des touches haut et bas
        move.z = vertical * speed;

        //Récupération des touches gauche et droite
        move.x = horizontal * speed;

        transform.position += move;
    }

    // Action
    void Action1()
    {
        bool isPressed = Input.GetButton("Action1");

        if(isPressed == true)
        {
            Debug.Log("Action1");
        }
    }

    // Action
    void Action2()
    {
        bool isPressed = Input.GetButton("Action2");

        if (isPressed == true)
        {
            Debug.Log("Action2");
        }
    }

    // Action
    void Action3()
    {
        bool isPressed = Input.GetButton("Action3");

        if (isPressed == true)
        {
            Debug.Log("Action3");
        }
    }

    // Action
    void Action4()
    {
        bool isPressed = Input.GetButton("Action4");

        if (isPressed == true)
        {
            Debug.Log("Action4");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        childInRange = true;
        
    }

    void OnTriggerExit(Collider other)
    {

        childInRange = false;

    }
}
