using La.Cantina.Controllers;
using La.Cantina.Manager;
using La.Cantina.Types;
using La.Cantina.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public  float speed = 10f;
    public float speedRotation = 0.1f;
    Child _childInRange = null;

    private const uint RESPONSE_FORCE = 236373571;
    private const uint RESPONSE_SHOUT = 1555546080;
    private const uint RESPONSE_CUDDLE = 2197215719;
    private const uint RESPONSE_CLEAN = 832448903;

    public AudioClip m_ResponseClipSuccess;
    public AudioClip m_ResponseClipFail;
    public AudioSource m_AudioSourceSFX;

    private VegetableConfig _vegetableCarried = null;

    [SerializeField] private MeshRenderer _body = null;
    [SerializeField] private PlayerCanvasController _playerCanvas   = null;
    [SerializeField] private Rigidbody _rigidBody = null;

    private int _joystickNumber;

    public void Initialize(int index, Material material)
    {
        _joystickNumber = index;
        _body.material  = material;
        _rigidBody      = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (_childInRange == null && IsPressedAction(2) == true && _vegetableCarried != null)
        {
            ThrowFood();
        }
    }

    private void FixedUpdate()
    {
        // Déplacement du joueur
#if ENABLE_KEYBOARD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
#else
        float horizontal = Input.GetAxis("Horizontal_P" + _joystickNumber.ToString());
        float vertical = Input.GetAxis("Vertical_P" + _joystickNumber.ToString());
#endif

        Move(horizontal, vertical);
    }

    // Déplacements
    private void Move(float horizontal, float vertical)
    {
        // Création d'un nouveau vecteur de déplacement
        Vector3 move = new Vector3();

        // Récupération des touches haut et bas
        move.z = vertical;

        //Récupération des touches gauche et droite
        move.x = horizontal;

        move = move.normalized * speed * Time.deltaTime;

        _rigidBody.MovePosition(transform.position + move);
        if (move != Vector3.zero) {

            _rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), speedRotation));
        }
    }

    // Check les boutons appuyés (1=A, 2=B, 3=Y, 4=X)
    private bool IsPressedAction(int button)
    {
#if ENABLE_KEYBOARD
        bool isPressed = Input.GetButtonDown("Action" + button.ToString() + "_Keyboard");
#else
        bool isPressed = Input.GetButtonDown("Action" + button.ToString() + "_P" + _joystickNumber.ToString());
#endif

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
        if (other.tag == "Children" && other.transform.parent.gameObject.tag == "SpawnerPlayer" + _joystickNumber.ToString())
        {
            other.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Cyan");

            _childInRange = other.GetComponent<Child>();

            ActionChild();

        }
        else if (other.tag == "FoodSlot")
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
                if (_childInRange.isOutOfTable == true && _childInRange.destination == Child.DestinationType.NONE)
                {
                    _childInRange.GoSit();
                }
                else
                {
                    ManageIncident(RESPONSE_FORCE);
                }
            }
        }
        else if(IsPressedAction(2) == true)
        {
            if (_vegetableCarried != null)
            {
                ThrowFood();
            }
            else
            {
                ManageIncident(RESPONSE_SHOUT);
            }
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
            bool result = _childInRange.SolveIncident(responseId);

            m_AudioSourceSFX.clip = result ? m_ResponseClipSuccess : m_ResponseClipFail;
            m_AudioSourceSFX.Play();

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

    private void ThrowFood()
    {
        if (_vegetableCarried != null)
        {
            _vegetableCarried = null;

            _playerCanvas.DisableFeedback();
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