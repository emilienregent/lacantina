using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.2f;
    bool childInRange = false;

    public int joystickNumber;


    // Update is called once per frame
    void Update()
    {
        // Déplacement du joueur
        float horizontal = Input.GetAxis("Horizontal_P" + joystickNumber.ToString());
        float vertical = Input.GetAxis("Vertical_P" + joystickNumber.ToString());

        Move(horizontal, vertical);

        if (childInRange == true)
        {
            DisplayAction();
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
        if (move != Vector3.zero) {

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.15F);
        }
    }


    void DisplayAction()
    {

    }
    // Action
    void Action1()
    {
        bool isPressed = Input.GetButton("Action1_P" + joystickNumber.ToString());

        if(isPressed == true)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    // Action
    void Action2()
    {
        bool isPressed = Input.GetButton("Action2_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Action
    void Action3()
    {
        bool isPressed = Input.GetButton("Action3_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    // Action
    void Action4()
    {
        bool isPressed = Input.GetButton("Action4_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // Action possible sur un enfant
        if (other.tag == "Children")
        {
            other.GetComponent<Renderer>().material.color = Color.cyan;
            childInRange = true;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Children")
        {
            other.GetComponent<Renderer>().material.color = Color.white;
            childInRange = false;
        }

    }
}
