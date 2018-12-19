using La.Cantina.Controllers;
using La.Cantina.Types;
using La.Cantina.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.2f;
    Child _childInRange = null;


    private VegetableConfig _vegetableCarried = null;

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

    // Bouton d'Action 1 (A)
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

    // Bouton d'Action 2 (B)
    bool Action2()
    {
        bool isPressed = Input.GetButton("Action2_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.red;
            return true;
        } else
        {
            return false;
        }
    }

    // Bouton d'Action 3 (Y)
    bool Action3()
    {
        bool isPressed = Input.GetButton("Action3_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.yellow;
            return true;
        } else
        {
            return false;
        }
    }

    // Bouton d'Action 4 (X)
    bool Action4()
    {
        bool isPressed = Input.GetButton("Action4_P" + joystickNumber.ToString());

        if (isPressed == true)
        {
            //_renderer.material.color = Color.blue;
            return true;
        } else
        {
            return false;
        }
    }





    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Children")
        {
            other.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Empty");
            _childInRange = null;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Children")
        {
            other.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Cyan");

            _childInRange = other.GetComponent<Child>();

            ActionChild();

        } else if (other.tag == "FoodSlot")
        {
            FoodSlotController foodSlot = other.GetComponent<FoodSlotController>();
            ActionFoodSlot(foodSlot);
        }

        
    }

    // Actions sur un enfant
    void ActionChild()
    {
        if(Action1() == true)
        {
            giveFood();
            ManageIncident(1);


        } else if(Action2() == true)
        {

        } else if(Action3() == true)
        {

        } else if (Action4() == true)
        {

        }
    }

    void giveFood()
    {

        if (_vegetableCarried != null)
        {
            _childInRange.GiveFood(_vegetableCarried);
            _vegetableCarried = null;

            _meshRenderer.material = Resources.Load<Material>("Materials/Empty");
        }
    }

    // Actions sur le chariot
    void ActionFoodSlot(FoodSlotController foodSlot)
    {
        if(Action1() == true)
        {
            VegetableConfig _vegetableConfig = foodSlot.Take();

            if (_vegetableConfig != null)
            {
                _vegetableCarried = _vegetableConfig;
                _meshRenderer.material = Resources.Load<Material>("Materials/" + _vegetableConfig.name.Replace(" ", ""));
            }
        }
    }

    // Réponse du joueur à un incident
    void ManageIncident(int responseId)
    {
        IncidentConfig _childrenIncident = _childInRange.m_currentIncident;


        if (_childrenIncident != null)
        {
            _childInRange.SolveIncident();
        }

        
    }
}
