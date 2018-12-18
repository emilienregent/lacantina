using La.Cantina.Controllers;
using La.Cantina.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.2f;
    bool childInRange = false;


    private VegetableConfig _vegetableConfig = null;

    private MeshRenderer _meshRenderer = null;

    public int joystickNumber;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        // Déplacement du joueur
        float horizontal = Input.GetAxis("Horizontal_P" + joystickNumber.ToString());
        float vertical = Input.GetAxis("Vertical_P" + joystickNumber.ToString());

        Move(horizontal, vertical);

        

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

    // Action
    bool Action1()
    {
        bool isPressed = Input.GetButton("Action1_P" + joystickNumber.ToString());

        if(isPressed == true)
        {
            //_renderer.material.color = Color.green;
            return true;
        } else
        {
            return false;
        }
    }

    // Action
    void Action2()
    {
        bool isPressed = Input.GetButton("Action2_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.red;
        }
    }

    // Action
    void Action3()
    {
        bool isPressed = Input.GetButton("Action3_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.yellow;
        }
    }

    // Action
    void Action4()
    {
        bool isPressed = Input.GetButton("Action4_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.blue;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Children")
        {
            other.GetComponent<Renderer>().material.color = Color.cyan;
            Action1();
            Action2();
            Action3();
            Action4();
        } else if (other.tag == "FoodSlot")
        {
            FoodSlotController foodSlot = other.GetComponent<FoodSlotController>();
            ActionFoodSlot(foodSlot);
        }

        
    }

    void ActionFoodSlot(FoodSlotController foodSlot)
    {
        if(Action1() == true)
        {
            _vegetableConfig = foodSlot.Take();
            if (_vegetableConfig != null)
            {
                _meshRenderer.material = Resources.Load<Material>("Materials/" + _vegetableConfig.name.Replace(" ", ""));
            }
        }
    }
}
