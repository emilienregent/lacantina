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
    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    [HideInInspector]
    public Seating allowedSeating = null;

    private PlayerManager _playerManager = null;

    [SerializeField]
    private ChildCanvasController _childCanvas = null;

    private Seat currentSeat = null;

    [SerializeField] private Slider  m_Slider = null;
    [SerializeField] private Image m_Slider_Foreground = null;

    public  int     m_timer         = 0;
    public  float   m_elapsedTime   = 0f;
    public  bool    m_isEating      = false;
    public  float   timeToIncidentModifier = 0;

    public IncidentConfig m_currentIncident = null;
    public VegetableConfig m_currentVegetable = null;

    public List<AudioClip> m_IncidentsSFX;
    public AudioSource m_AudioSourceSFX;

    // Claim a seat in the assigned set and move to its position.
    public void GoSit()
    {
        if (currentSeat == null) 
            currentSeat = allowedSeating.GetEmptySeat();

        if (currentSeat != null)
        {
            currentSeat.isClaimed = true;
            _navMeshAgent.SetDestination(currentSeat.transform.position);
        }
    }

    // Initialize for the set player
    public void InitForPlayer(PlayerManager player)
    {
        _playerManager = player;
    }

    public void Awake()
    {
        m_Slider.value = 0f;
        m_Slider_Foreground.color = Color.green;

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
    }

    // Give vegetable to a kid
    // If he is currently in an incident, he doesn't start to eat
    public bool GiveFood(VegetableConfig vegetable)
    {
        Debug.Log("Give Food : " + vegetable.name);

        if(m_currentIncident == null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
            m_currentVegetable = vegetable;
            _childCanvas.EnableTimer(true);
        }

        return m_currentIncident == null;
    }

    private void EndMeal()
    {
        _playerManager.UpdateScore(m_currentVegetable.points, true);

        m_isEating          = false;
        m_currentVegetable  = null;
        m_Slider.value      = 0;
        m_timer             = 0;
        _childCanvas.EnableTimer(false);
    }

    // Solve an incident with a kid
    // If he has vegetable to eat, he starts to eat again
    public bool SolveIncident(uint responseId)
    {
        ResponseConfig response = GameManager.instance.responseIdToConfig[responseId];

        Debug.Log("Solve incident with: " + response.name);

        if (m_currentIncident != null)
        {
            UnityEngine.Assertions.Assert.IsTrue(response.incidentIdToResult.ContainsKey(m_currentIncident.id), "Can't find result for response " + response.name + " and " + m_currentIncident.name);

            if(response.incidentIdToResult[m_currentIncident.id] == false)
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

            } else
            {
                timeToIncidentModifier = response.time;
                Debug.Log("bonus " + response.time);
                _playerManager.UpdateScore(response.points, false);
                return false;
            }

            _childCanvas.DisableFeedback();
        }

        m_currentIncident = null;

        if(m_currentVegetable != null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
            m_Slider_Foreground.color = Color.green;
        }

        _playerManager.UpdateScore(response.points, true);
        return true;
    }

    // Start a random incident
    private void StartIncident()
    {
        // Reset Malus/Bonus
        timeToIncidentModifier = 0;
        int rand = UnityEngine.Random.Range(0, GameManager.instance.incidentIdToConfig.Keys.Count);
        int count = 0;

        foreach (KeyValuePair<uint, IncidentConfig> pair in GameManager.instance.incidentIdToConfig)
        {
            if (rand == count)
            {
                m_currentIncident = pair.Value;
                m_isEating = false;
                m_Slider_Foreground.color = Color.red;

                _childCanvas.EnableFeedback("Incident/incident_" + m_currentIncident.name.ToLower(), true);
                AudioClip incidentClip = m_IncidentsSFX[UnityEngine.Random.Range(0, m_IncidentsSFX.Count)];
                m_AudioSourceSFX.clip = incidentClip;
                m_AudioSourceSFX.Play();

                Debug.Log("Start new incident : " + m_currentIncident.name);
                break;
            }

            count++;
        }
    }
}