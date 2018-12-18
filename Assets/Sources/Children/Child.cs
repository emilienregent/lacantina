using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using La.Cantina.Types;
using La.Cantina.Manager;

public class Child : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    private Seat currentSeat = null;

    public float m_elapsedTime = 0f;
    public int m_triggerDelay = 2;
    private Slider m_Slider;
    public bool m_isEating = false;
    public int m_timer = 0;

    public IncidentConfig m_currentIncident = null;
    public VegetableConfig m_currentVegetable = null;

    public void GoSit(Seating seats)
    {
        currentSeat = seats.GetEmptySeat();

        if (currentSeat != null)
        {
            currentSeat.isClaimed = true;
            _navMeshAgent.SetDestination(currentSeat.seatTransform.position);
        }
    }

    public void Awake()
    {
        m_Slider = GetComponentInChildren<Slider>();
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

                // If the incident trigger delay is reached
                if(m_timer%m_triggerDelay == 0)
                {
                    // We start an incident
                    StartIncident();
                    SolveIncident();

                // Else we process the progression of the "eat" action
                } else
                {
                    // TODO : Progression of the gauge to defined (currently 10% / s)
                    m_Slider.value += 10;
                    // If the gauge is filled, the kid has finished to eat
                    if(m_Slider.value >= m_Slider.maxValue)
                    {
                        m_isEating = false;
                        m_currentVegetable = null;
                    }
                }
            }
        }
    }

    // Give vegetable to a kid
    // If he is currently in an incident, he doesn't start to eat
    public void GiveFood(VegetableConfig vegetable)
    {
        Debug.Log("Give Food : " + vegetable.name);
        if(m_currentIncident == null)
        {
            m_isEating = true;
        }
        m_elapsedTime = 0f;
        m_currentVegetable = vegetable;
    }

    // Solve an incident with a kid
    // If he has vegetable to eat, he starts to eat again
    public void SolveIncident()
    {
        Debug.Log("Solve incident");
        m_currentIncident = null;
        if(m_currentVegetable != null)
        {
            m_isEating = true;
            m_elapsedTime = 0f;
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

                Debug.Log("Start new incident : " + m_currentIncident.name);
            }

            count++;
        }
    }
}