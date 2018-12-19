using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using La.Cantina.Types;
using La.Cantina.Manager;
using System;

public class Child : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    [HideInInspector]
    public Seating allowedSeating = null;

    [HideInInspector]
    public int playerNumber = 0;

    private Seat currentSeat = null;

    private Slider  m_Slider;
    [SerializeField] private Image m_Slider_Background = null;
    [SerializeField] private Image m_Slider_Foreground = null;

    public  int     m_timer         = 0;
    public  float   m_elapsedTime   = 0f;
    public  bool    m_isEating      = false;

    public IncidentConfig m_currentIncident = null;
    public VegetableConfig m_currentVegetable = null;

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
    public void InitForPlayer(int player)
    {
        playerNumber = player;


    }

    public void Awake()
    {
        m_Slider_Background.enabled = false;

        m_Slider = GetComponentInChildren<Slider>();        
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
                    m_isEating = false;
                    m_currentVegetable = null;
                }
                // If the incident trigger delay is reached
                else if (m_timer != 0 && (m_timer % m_currentVegetable.timeToIncident == 0))
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
            m_Slider_Background.enabled = true;
        }

        return m_currentIncident == null;
    }

    // Solve an incident with a kid
    // If he has vegetable to eat, he starts to eat again
    public void SolveIncident(uint responseId)
    {
        Debug.Log("Solve incident");

        ResponseConfig response = GameManager.instance.responseIdToConfig[responseId];

        if(m_currentIncident != null)
        {
            UnityEngine.Assertions.Assert.IsTrue(response.incidentIdToResult.ContainsKey(m_currentIncident.id), "Can't find result for response " + response.name + " and " + m_currentIncident.name);
            if(response.incidentIdToResult[m_currentIncident.id] == false)
            {
                Debug.Log("Wrong response to incident " + response.name);
                return;
            }
        }

        m_currentIncident = null;

        if(m_currentVegetable != null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
            m_Slider_Foreground.color = Color.green;
        }
    }

    // Start a random incident
    private void StartIncident()
    {
        int rand = UnityEngine.Random.Range(0, GameManager.instance.incidentIdToConfig.Keys.Count);
        int count = 0;

        foreach (KeyValuePair<uint, IncidentConfig> pair in GameManager.instance.incidentIdToConfig)
        {
            if (rand == count)
            {
                m_currentIncident = pair.Value;
                m_isEating = false;
                m_Slider_Foreground.color = Color.red;

                Debug.Log("Start new incident : " + m_currentIncident.name);
                break;
            }

            count++;
        }
    }
}