using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using La.Cantina.Types;
using La.Cantina.Manager;
using System;
using La.Cantina.UI;

public class Child : MonoBehaviour
{
    public enum DestinationType
    {
        NONE,
        CHAIR,
        START,
        RANDOM
    }

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    [HideInInspector]
    public Seating allowedSeating = null;

    private PlayerManager _playerManager = null;

    public CapsuleCollider _bodyCollider = null;
    public BoxCollider _tailCollider = null;

    [SerializeField]
    private ChildCanvasController _childCanvas = null;

    private Seat currentSeat = null;
    private bool m_isOutOfTable = true;
    public bool isOutOfTable { get { return m_isOutOfTable; } }

    [SerializeField] private Slider  m_Slider = null;
    [SerializeField] private Image m_Slider_Foreground = null;

    public  int     m_timer         = 0;
    public  int     m_waiting_timer = 0;
    public  float   m_elapsedTime   = 0f;
    public  bool    m_isEating      = false;
    public  float   timeToIncidentModifier = 0;
    public  int     timeBeforeLeaving = 10;
    public  DestinationType destination = DestinationType.NONE;

    private Vector3 _lookAtWhenSitting = Vector3.zero;

    public IncidentConfig m_currentIncident = null;
    public VegetableConfig m_currentVegetable = null;

    public List<AudioClip> m_IncidentsSFX;
    public AudioSource m_AudioSourceSFX;

    // Claim a seat in the assigned set and move to its position.
    public void GoSit()
    {
        _bodyCollider.enabled = false;
        if (currentSeat == null) 
            currentSeat = allowedSeating.GetEmptySeat();

        if (currentSeat != null)
        {
            currentSeat.isClaimed = true;
            _navMeshAgent.SetDestination(currentSeat.transform.position);
            destination = DestinationType.CHAIR;

            _lookAtWhenSitting.x = currentSeat.LookAtWhenSitting.position.x;
            _lookAtWhenSitting.z = currentSeat.LookAtWhenSitting.position.z;
        }
    }

    // End game scenario : Cancel everything and go to the position
    public void CancelAndLeave(Vector3 startPos)
    {
        _navMeshAgent.SetDestination(startPos);
        destination = DestinationType.START;
        m_currentIncident = null;
        EndMeal();
    }

    // Initialize for the set player
    public void InitForPlayer(PlayerManager player)
    {
        _playerManager = player;
    }

    public void Awake()
    {
        m_Slider.value = 0f;
        m_Slider_Foreground.color = _playerManager.playerColor;
        
        if (GameManager.instance.isReady == false)
        {
            GameManager.instance.Initialized += OnGameInitialized;
        }
        else
        {
            OnGameInitialized(GameManager.instance, null);
        }
    }

    private void OnGameInitialized(object sender, EventArgs eventArgs)
    {
#if DEBUG_CHILD
        int rand    = UnityEngine.Random.Range(0, GameManager.instance.incidentIdToConfig.Keys.Count);
        int count   = 0;

        foreach (KeyValuePair<uint, VegetableConfig> pair in GameManager.instance.vegetableIdToConfig)
        {
            if (rand == count)
            {
                GiveFood(pair.Value);
                break;
            }

            count++;
        }
#endif
    }

    public void Update()
    {
        // If the kid is eating and he isn't in an incident
        if(m_isEating == true && m_currentIncident == null)
        {
            // Each second
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= 1f)
            {
                m_elapsedTime = m_elapsedTime % 1f;
                m_timer++;

                // Progression based on the vegetable configuration
                m_Slider.value = (100 / m_currentVegetable.timeToEat) * m_timer;


                // If the gauge is filled, the kid has finished to eat
                if (m_timer >= m_currentVegetable.timeToEat)
                {
                    EndMeal();
                }
                // If the incident trigger delay is reached
                else if (m_timer != 0 && ((m_timer + timeToIncidentModifier) % (m_currentVegetable.timeToIncident + timeToIncidentModifier) == 0))
                {
                    // We start an incident
                    StartIncident();
                }
            }
        }

        // If the kid is waiting for food at table
        if(m_isEating == false && m_isOutOfTable == false && m_currentIncident == null)
        {
             m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= 1f)
            {
                m_elapsedTime = m_elapsedTime % 1f;
                m_waiting_timer++;

                if(m_waiting_timer > timeBeforeLeaving)
                {
                    LeaveSeat();
                }
            }
        }

        // Actions to perform when reaching a destination
        if (
            destination != DestinationType.NONE && 
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance
        )
        {
            switch (destination)
            {
                case DestinationType.START:
                    gameObject.SetActive(false);
                    break;

                case DestinationType.CHAIR:
                    Debug.Log("est arrivé à table");
                    m_isOutOfTable = false;
                    _tailCollider.enabled = true;
                    _lookAtWhenSitting.y = transform.position.y;
                    break;

                case DestinationType.RANDOM:
                    _bodyCollider.enabled = true;
                    break;
            }

            destination = DestinationType.NONE;
        }

        if (!m_isOutOfTable)
            transform.LookAt(_lookAtWhenSitting);
    }

    // Give vegetable to a kid
    // If he is currently in an incident, he doesn't start to eat
    public bool GiveFood(VegetableConfig vegetable)
    {
        Debug.Log("Give Food : " + vegetable.name);

        if(m_isOutOfTable == true)
        {
            return false;
        }

        if(m_currentIncident == null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
            m_currentVegetable = vegetable;
            _childCanvas.EnableSlider(true);
            _childCanvas.EnableFeedback("Food/food_" + m_currentVegetable.name.ToLower(), true, true);
        }

        return m_currentIncident == null;
    }

    private void EndMeal()
    {
        if (m_currentVegetable != null)
        {
            _playerManager.UpdateScore(m_currentVegetable.points, true);

            m_isEating          = false;
            m_currentVegetable  = null;
            m_Slider.value      = 0;
            m_timer             = 0;
            m_waiting_timer     = 0;

            _childCanvas.DisableFeedback();
        }
    }

    // Solve an incident with a kid
    // If he has vegetable to eat, he starts to eat again
    public bool SolveIncident(uint responseId)
    {
        bool result = false;
        ResponseConfig response = GameManager.instance.responseIdToConfig[responseId];

        Debug.Log("Solve incident with: " + response.name);

        if (m_currentIncident != null)
        {
            UnityEngine.Assertions.Assert.IsTrue(response.incidentIdToResult.ContainsKey(m_currentIncident.id), "Can't find result for response " + response.name + " and " + m_currentIncident.name);

            result = response.incidentIdToResult[m_currentIncident.id];

            if (result == false)
            {

                Debug.Log("Wrong response to incident: " + response.name);

                Debug.Log("malus " + response.time);
                // If Malus is more or equal to timeToIncident, we start a new incident
                if(m_currentVegetable.timeToIncident - response.time <= 0)
                {
                    StartIncident();
                    _playerManager.UpdateScore(response.points, false);
                    return false;
                }
                else
                {
                    timeToIncidentModifier = -response.time;
                }

            }
            else
            {
                Debug.Log("bonus " + response.time);
                timeToIncidentModifier = response.time;
            }

            _childCanvas.DisableFeedback();
            _playerManager.UpdateScore(response.points, result);
        }

        m_currentIncident = null;

        if(m_currentVegetable != null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
            m_Slider_Foreground.color = _playerManager.playerColor;
            _childCanvas.EnableSlider(true);
            _childCanvas.EnableFeedback("Food/food_" + m_currentVegetable.name.ToLower(), true, true);
        }
        
        return result;
    }

    // Start a random incident
    private void StartIncident()
    {
        // Reset Malus/Bonus
        timeToIncidentModifier = 0;
        int rand = UnityEngine.Random.Range(0, GameManager.instance.incidentIdToConfig.Keys.Count);
        int count = 0;
        m_waiting_timer = 0;

        foreach (KeyValuePair<uint, IncidentConfig> pair in GameManager.instance.incidentIdToConfig)
        {
            if (rand == count)
            {
                m_currentIncident = pair.Value;
                m_isEating = false;

                _childCanvas.EnableFeedback("Incident/incident_" + m_currentIncident.name.ToLower(), false,  true);
                AudioClip incidentClip = m_IncidentsSFX[UnityEngine.Random.Range(0, m_IncidentsSFX.Count)];
                m_AudioSourceSFX.clip = incidentClip;
                m_AudioSourceSFX.Play();

                Debug.Log("Start new incident : " + m_currentIncident.name);
                break;
            }

            count++;
        }
    }

    // Leave the seat and go to random point
    private void LeaveSeat()
    {
        System.Random rnd = new System.Random();
        
        int x1 = rnd.Next(-95, 95);
        int z1 = 0;
        bool isPositiveZ;
        if (x1 < 80 || x1 > -80)
        {
            z1 = rnd.Next(80, 95);
            isPositiveZ = rnd.NextDouble() >= 0.5;Debug.Log(isPositiveZ);
            if (isPositiveZ == false)
            {
                z1 = -z1;
            }
        } else
        {
            z1 = rnd.Next(-95, 95);
        }
        float x = (float)x1 / 10;
        float z = (float)z1 / 10;


        Vector3 randomPoint = new Vector3(x, 0, z);

        m_isEating = false;
        m_isOutOfTable = true;

        _navMeshAgent.SetDestination(randomPoint);
        m_waiting_timer = 0;

        _tailCollider.enabled = false;
        destination = DestinationType.RANDOM;
    }

}