using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientTracker : MonoBehaviour
{
    public GameObject patient;
    void Update()
    {
        if(patient)
        {
            gameObject.transform.position = patient.transform.position + new Vector3(0, 0.75f, 0);   
        }
    }
}
