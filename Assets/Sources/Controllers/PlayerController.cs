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

    private const uint RESPONSE_FORCE = 236373571;
    private const uint RESPONSE_SHOUT = 1555546080;
    private const uint RESPONSE_CUDDLE = 2197215719;
    private const uint RESPONSE_CLEAN = 832448903;


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



    // Check les boutons appuyés (1=A, 2=B, 3=Y, 4=X)
    bool isPressedAction(int button)
    {
        bool isPressed = Input.GetButton("Action" + button.ToString() + "_P" + joystickNumber.ToString());

        return isPressed;
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
        if(isPressedAction(1) == true)
        {
            giveFood();
            _childInRange.SolveIncident(RESPONSE_FORCE);


        } else if(isPressedAction(2) == true)
        {
            _childInRange.SolveIncident(RESPONSE_SHOUT);
        } else if(isPressedAction(3) == true)
        {
            _childInRange.SolveIncident(RESPONSE_CUDDLE);
        } else if (isPressedAction(4) == true)
        {
            _childInRange.SolveIncident(RESPONSE_CLEAN);
        }
    }

    void giveFood()
    {

        if (_vegetableCarried != null)
        {
            if(_childInRange.GiveFood(_vegetableCarried) == true)
            {
                _vegetableCarried = null;

                _meshRenderer.material = Resources.Load<Material>("Materials/Empty");
            }
        }
    }

    // Actions sur le chariot
    void ActionFoodSlot(FoodSlotController foodSlot)
    {
        if(isPressedAction(1) == true)
        {
            VegetableConfig _vegetableConfig = foodSlot.Take();

            if (_vegetableConfig != null)
            {
                _vegetableCarried = _vegetableConfig;
                _meshRenderer.material = Resources.Load<Material>("Materials/" + _vegetableConfig.name.Replace(" ", ""));
            }
        }
    }

}
