using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PatientSpawner : MonoBehaviour
{
    [SerializeField] private ReputationReward rep;
    [SerializeField] private GameObject entrance;
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private GameObject patientParent;
    private int collumnNumber = 0;
    private float amountcollumn = 0;
    private Vector3 patientPosition;
    private Vector3 CurrentpatientPosition;
    private GameObject player;

    //Organizer
    private float amountOrganize;
    private int collumnNumberOrganize;
    private Vector3 patientPositionOrganize;
    private Vector3 CurrentpatientPositionOrganize;
    [SerializeField] private List<GameObject> patientOrganizerList = new List<GameObject>();

    //DeathBar 
    [SerializeField] private GameObject patienceBarPrefab;
    [SerializeField] private GameObject patienceBarParent;
    private List<GameObject> globalPatienceBarList = new List<GameObject>();
    private int deathCounter = 0;
    
    [SerializeField] private int repLevel;
    private GameObject repBar;

    [SerializeField] private List<GameObject> patientList = new List<GameObject>();
    private List<GameObject> globalPatientList = new List<GameObject>();

    private float newPatientTimer;

    [SerializeField] private float patientTimerLimit = 15f;
    private float initialTimer;

    private GameObject stateManager;

    public UnityEvent<GameObject> murderPatient = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> curePatient = new UnityEvent<GameObject>();

    [SerializeField] private GameObject losePrefab;
    [SerializeField] private GameObject losePrefabHold;

    // Start is called before the first frame update
    void Start()
    {
        initialTimer = patientTimerLimit;
        patientPosition = entrance.transform.position;
        newPatientTimer = patientTimerLimit - Time.deltaTime*5;
        stateManager = FindObjectOfType<GameStateManager>().gameObject;
        player = GameObject.FindWithTag("Player");
        murderPatient.AddListener(MurderPatient);
        curePatient.AddListener(PatientCured);
        patienceBarPrefab.GetComponent<PatienceBar>().SetMaxPatience(100);
        repBar = FindObjectOfType<ReputationBar>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stateManager.GetComponent<GameStateManager>().paused)
        {
            AddMemberToLine();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RemovePatientFromLine();
            }
        }
    }

    public void SetMaxPatienceBar(float num)
    {
        patienceBarPrefab.GetComponent<PatienceBar>().SetMaxPatience(num);
    }
    public float GetMaxPatienceBar()
    {
        return patienceBarPrefab.GetComponent<PatienceBar>().maxPatience;
    }

    private void AddMemberToLine()
    {
        newPatientTimer += Time.deltaTime;
        repLevel = repBar.GetComponent<ReputationBar>().reputationLevel;
        patientTimerLimit = initialTimer - repLevel/2;
        if (newPatientTimer >= patientTimerLimit)
        {
            newPatientTimer = 0;
            int random = Random.Range(1, 10);
            int spawnNum;
            if (random <= 7)
            {
                spawnNum = 1;
            }
            else
            {
                spawnNum = 2;
            }

            for (int i = 0; i < spawnNum; i++)
            {
                GameObject newPatient = Instantiate(patientPrefab, patientParent.transform);
                patientList.Add(newPatient);
                globalPatientList.Add(newPatient);
                GameObject newBar = Instantiate(patienceBarPrefab, patienceBarParent.transform);
                newBar.GetComponent<PatientTracker>().patient = newPatient;
                globalPatienceBarList.Add(newBar);
                CheckForNewLine();
                if (amountcollumn == 0)
                {
                    newPatient.transform.position = patientPosition + new Vector3(-1.25f, 0f, 0);
                    patientPosition = newPatient.transform.position;
                    CurrentpatientPosition = newPatient.transform.position;
                }
                else
                {
                    //int x = patientList.Count / 7;
                    newPatient.transform.position = patientPosition + new Vector3(-1.25f, CheckAmountInLine() * (1.25f * (amountcollumn - 1f)), 0);
                    CurrentpatientPosition = newPatient.transform.position;
                }
                amountcollumn += 1;
            }
        }
    }



    public void RemovePatientFromLine()
    {
        GameObject patient = patientList.First();
        patientList.Remove(patient);
        amountOrganize = 0;
        collumnNumberOrganize = 0;
        patientPositionOrganize = entrance.transform.position;
        CurrentpatientPositionOrganize = entrance.transform.position;
        patientOrganizerList.Clear();
        OrganizeLine();
        amountcollumn = amountOrganize;
        collumnNumber = collumnNumberOrganize;
        CurrentpatientPosition = CurrentpatientPositionOrganize;
        patientPosition = patientPositionOrganize;
        collumnNumber = collumnNumberOrganize;

        PatientMovemnt patientMovemnt = patient.GetComponent<PatientMovemnt>();
        patientMovemnt.patientState = 2;
    }

    private void OrganizeLine()
    {
        foreach (GameObject patient in patientList)
        {
            patientOrganizerList.Add(patient);
            CheckForNewLine2();
            if (amountOrganize == 0)
            {
                patient.transform.position = patientPositionOrganize + new Vector3(-1.25f, 0f, 0);
                patientPositionOrganize = patient.transform.position;
                CurrentpatientPositionOrganize = patient.transform.position;
            }
            else
            {
                //int x = patientList.Count / 7;
                patient.transform.position = patientPositionOrganize + new Vector3(-1.25f, CheckAmountInLine2() * (1.25f * (amountOrganize - 1f)), 0);
                CurrentpatientPositionOrganize = patient.transform.position;
            }
            amountOrganize += 1;
        }
    }

    private int CheckAmountInLine()
    {
        int i = patientList.Count / 5;
        if (collumnNumber >= 0)
        {
            i = (patientList.Count + 1) / 6;
        }

        CheckForNewLine();
        if (i % 2 == 0)
        {
            //even
            return 1;
        }
        //odd
        return -1;
    }
    private int CheckAmountInLine2()
    {
        int i = patientOrganizerList.Count / 5;
        if (collumnNumberOrganize >= 0)
        {
            i = (patientOrganizerList.Count + 1) / 6;
        }

        CheckForNewLine2();
        if (i % 2 == 0)
        {
            //even
            return 1;
        }
        //odd
        return -1;
    }

    private void CheckForNewLine()
    {
        int i = patientList.Count / 5;
        if (collumnNumber >= 0)
        {
            i = (patientList.Count + 1) / 6;
        }
        if (i != collumnNumber)
        {
            collumnNumber = i;
            patientPosition = CurrentpatientPosition;
            amountcollumn = 0f;
        }
    }
    private void CheckForNewLine2()
    {
        int i = patientOrganizerList.Count / 5;
        if (collumnNumberOrganize >= 0)
        {
            i = (patientOrganizerList.Count + 1) / 6;
        }
        if (i != collumnNumberOrganize)
        {
            collumnNumberOrganize = i;
            patientPositionOrganize = CurrentpatientPositionOrganize;
            amountOrganize = 0f;
        }
    }

    public PatienceBar GetBarWithPatient(GameObject patient)
    {
        int index = globalPatientList.IndexOf(patient);
        GameObject bar = globalPatienceBarList[index];
        return bar.GetComponent<PatienceBar>();
    }

    private void PatientCured(GameObject patient)
    {
        int index = globalPatientList.IndexOf(patient);
        GameObject bar = globalPatienceBarList[index];
        globalPatientList.Remove(patient);
        globalPatienceBarList.Remove(bar);
        rep.AddRep(bar.GetComponent<PatienceBar>().GetPatience());

        Destroy(bar);
        Destroy(patient);
    }

    private void MurderPatient(GameObject bar)
    {
        int index = globalPatienceBarList.IndexOf(bar);
        GameObject patient = globalPatientList[index];
        globalPatienceBarList.Remove(bar);
        globalPatientList.Remove(patient);
        if(patientList.Contains(patient))
        {
            RemovePatientFromLine();
        }
        patient.GetComponent<PatientMovemnt>().patientState = -1;
        Destroy(bar);
        Destroy(patient);
        deathCounter++;
        if (deathCounter >= 3)
        {
            Instantiate(losePrefab, losePrefabHold.transform);
        }
    }
}
