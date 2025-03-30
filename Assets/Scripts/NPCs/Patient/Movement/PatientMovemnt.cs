using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PatientMovemnt : MonoBehaviour
{
    // Has the stats for the patient
    // 1 -In Line
    // 2 -Following player
    // 3 -In bed
    public int patientState = 1;

    private GameObject player;
    private GameObject bed;
    private GameObject theGoodPlace;
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (patientState == 2)
        {
            Vector3 offSet = new Vector2(-1.2f, -0.3f);
            Vector2 target = player.transform.position + offSet;

            agent.speed = 8f;
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        else if (patientState == 3)
        {
            agent.SetDestination(bed.transform.position);
        }
        else if (patientState == 4)
        {
            agent.SetDestination(theGoodPlace.transform.position);

            if (agent.hasPath && !agent.pathPending && agent.remainingDistance != Mathf.Infinity && agent.remainingDistance < 1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    public void MoveToBed(GameObject bed)
    {
        patientState = 3;
        this.bed = bed;
    }

    public void SendPatientToHome(GameObject heaven)
    {
        theGoodPlace = heaven;
        patientState = 4;

    }
}

