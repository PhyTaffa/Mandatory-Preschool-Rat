using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteraction : MonoBehaviour, IInteractable
{
    private bool isPatientOnBed = false;

    public void Interact()
    {
        //Put funciton call here to put the patient in bed
        Debug.Log("Interacted with the a Bed");

        PatientInteraction patientInteraction = FindObjectOfType< PatientInteraction >();
        Medication meds = FindObjectOfType<Medication>();
        PatientMovemnt patientMovemnt = FindObjectOfType< PatientMovemnt>();

        if (isPatientOnBed && meds.currentMediHeld > 0)
        {
            Debug.Log("Curring Patient");
        }

        if (patientInteraction.playerHasPatient)
        {
            Debug.Log("Patient is on bed");

            isPatientOnBed = true;

            patientInteraction.playerHasPatient = false;
            patientMovemnt.MoveToBed(gameObject);
        }
    }
}
