using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PatientMovemnt : MonoBehaviour
{
    // Has the stats for the patient
    // -1 -Is dead
    // 1 -In Line
    // 2 -Following player
    // 3 -In bed
    // 4 -Is cured
    public int patientState = 1;

    private GameObject player;
    private GameObject bed;
    private GameObject theGoodPlace;
    [SerializeField] private NavMeshAgent agent;
    private GameObject cureMe;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent.enabled = true;
        cureMe = FindObjectOfType<PatientSpawner>().gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patientState == 2)
        {
            //animator.SetBool("walkup", )
            Vector3 offSet = new Vector2(-1.2f, -0.3f);
            Vector2 target = player.transform.position + offSet;
            AnimationBoolSetter(new Vector2(transform.position.x, transform.position.y) - target);
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
            agent.isStopped = false;

            agent.SetDestination(theGoodPlace.transform.position);

            if (agent.hasPath && !agent.pathPending && agent.remainingDistance != Mathf.Infinity && agent.remainingDistance < 1f)
            {
                patientState = 5;
                cureMe.GetComponent<PatientSpawner>().curePatient.Invoke(gameObject);
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

    private void AnimationBoolSetter(Vector2 dir)
    {
        float x = Mathf.Round(dir.normalized.x);
        float y = Mathf.Round(dir.normalized.y);

        if(x == 0 && y == 0)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", false);
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
        }
        if(x == -1 && y == 0)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", false);
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", true);
        }
        if(x == 1 && y == 0)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", false);
            animator.SetBool("MovingLeft", true);
            animator.SetBool("MovingRight", false);
        }
        if(x == 0 && y == -1)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("MovingUp", true);
            animator.SetBool("MovingDown", false);
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
        }
        if(x == 0 && y == 1)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("MovingUp", false);
            animator.SetBool("MovingDown", true);
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
        }
        
    }
}

