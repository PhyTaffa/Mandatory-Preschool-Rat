using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedsInteraction : MonoBehaviour, IInteractable
{
    public void Interact(GameObject obj)
    {
        //Put funciton call here to get meds
        Debug.Log("Interacted with the a MedShed");

        Medication medication = FindObjectOfType<Medication>();
        medication.AddCurrentMedication(1);
    }
}
