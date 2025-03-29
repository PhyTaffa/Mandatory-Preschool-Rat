using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject entrance;
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private GameObject patientParent;
    private int collumnNumber = 0;
    private float amountcollumn = 0;
    private Vector3 patientPosition;
    private Vector3 CurrentpatientPosition;

    [SerializeField] private int repLevel;
    
    [SerializeField] private List<GameObject> patientList = new List<GameObject>();

    private float newPatientTimer;

    private float patientTimerLimit = 35f;
    // Start is called before the first frame update
    void Start()
    {
        patientPosition = entrance.transform.position;
        newPatientTimer = patientTimerLimit;
    }

    // Update is called once per frame
    void Update()
    {
        AddMemberToLine();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemovePatientFromLine(patientList.First());
        }
    }

    private void AddMemberToLine()
    {
        newPatientTimer += Time.deltaTime;
        patientTimerLimit = 35 - repLevel;
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
                CheckForNewLine();
                if (amountcollumn == 0)
                {
                    newPatient.transform.position = patientPosition + new Vector3(-1.25f, 0f, 0);
                    patientPosition = newPatient.transform.position;
                    CurrentpatientPosition = newPatient.transform.position;
                }
                // else if (collumnNumber > 0 && amountcollumn == 1)
                // {
                //     newPatient.transform.position = patientPosition + new Vector3(-1.25f, 0f, 0);
                //     patientPosition = newPatient.transform.position;
                //     CurrentpatientPosition = newPatient.transform.position;
                // }
                 else if (collumnNumber > 0 && amountcollumn > 0)
                 {
                     newPatient.transform.position = patientPosition + new Vector3(-1.25f, CheckAmountInLine() * (1.25f * (amountcollumn-1)), 0);
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

    public void RemovePatientFromLine(GameObject patient)
    {
        patientList.Remove(patient);
        Destroy(patient);
        OrganizeLine();
    }

    private void OrganizeLine()
    {
        int i = 1;
        foreach (GameObject patient in patientList)
        {
            if (i == 1)
            {
                patient.transform.position = patientPosition + new Vector3(-2f, 0f, 0);
            }
            else
            {
                patient.transform.position = patientPosition + new Vector3(-2.75f, 1.25f * (i - 2), 0);
            }
            i++;
        }
    }

    private int CheckAmountInLine()
    {
        int i = patientList.Count/5;
        if(collumnNumber >= 0)
        {
             i = (patientList.Count+1)/6;  
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

    private void CheckForNewLine()
    {
        int i = patientList.Count/5;
        if(collumnNumber >= 0)
        {
             i = (patientList.Count+1)/6;  
        }
        if (i != collumnNumber)
        {
            collumnNumber = i;
            patientPosition = CurrentpatientPosition;
            amountcollumn = 0f;
        }
    }
}
