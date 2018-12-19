using La.Cantina.Controllers;
using La.Cantina.Manager;
using La.Cantina.Types;
using La.Cantina.UI;
using System.Collections;
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

    [SerializeField] private PlayerCanvasController _playerCanvas   = null;

    public int joystickNumber;

    private int _score = 0;
    public int score { get { return _score; } }

    // Update is called once per frame
    private void Update()
    {
        // Déplacement du joueur
        float horizontal = Input.GetAxis("Horizontal_P" + joystickNumber.ToString());
        float vertical = Input.GetAxis("Vertical_P" + joystickNumber.ToString());

        Move(horizontal, vertical);
    }

    // Déplacements
    private void Move(float horizontal, float vertical)
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
    private bool IsPressedAction(int button)
    {
        bool isPressed = Input.GetButtonDown("Action" + button.ToString() + "_P" + joystickNumber.ToString());

        return isPressed;
    }

    private void OnTriggerExit(Collider other)
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
    private void ActionChild()
    {
        if(IsPressedAction(1) == true)
        {
            if (_vegetableCarried != null)
            {
                GiveFood();
            }
            else
            {
                ManageIncident(RESPONSE_FORCE);
            }
        }
        else if(IsPressedAction(2) == true)
        {
            ManageIncident(RESPONSE_SHOUT);
        }
        else if(IsPressedAction(3) == true)
        {
            ManageIncident(RESPONSE_CUDDLE);
        }
        else if (IsPressedAction(4) == true)
        {
            ManageIncident(RESPONSE_CLEAN);
        }
    }

    private void ManageIncident(uint responseId)
    {
        if (_childInRange.m_currentIncident != null)
        {
            int points = _childInRange.SolveIncident(responseId);

            _score += points;

            _playerCanvas.EnableFeedback("Response/response_" + GameManager.instance.responseIdToConfig[responseId].name.ToLower());
        }
    }

    private void TakeFood(FoodSlotController foodSlot)
    {
        if (_vegetableCarried == null)
        {
            VegetableConfig vegetableConfig = foodSlot.Take();

            if (vegetableConfig != null)
            {
                _vegetableCarried = vegetableConfig;

                _playerCanvas.EnableFeedback("Food/food_" + vegetableConfig.name.ToLower(), true);
            }
        }
    }

    private void GiveFood()
    {
        if (_vegetableCarried != null)
        {
            if(_childInRange.GiveFood(_vegetableCarried) == true)
            {
                _vegetableCarried = null;

                _playerCanvas.DisableFeedback();
            }
        }
    }

    // Actions sur le chariot
    private void ActionFoodSlot(FoodSlotController foodSlot)
    {
        if(IsPressedAction(1) == true)
        {
            TakeFood(foodSlot);
        }
    }
}