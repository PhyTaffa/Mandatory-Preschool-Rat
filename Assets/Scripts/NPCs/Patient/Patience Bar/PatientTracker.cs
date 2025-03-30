using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientTracker : MonoBehaviour
{
    public GameObject patient;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(patient)
        {
            gameObject.transform.position = patient.transform.position + new Vector3(0, 1.25f, 0);   
        }
    }
}
