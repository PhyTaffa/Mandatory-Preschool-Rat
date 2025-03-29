using UnityEngine;

public class PatientInteraction : MonoBehaviour, IInteractable
{
    private bool playerHasPatient = false;
    public void Interact()
    {
        Debug.Log($"Interacted with the patient Line");

        if (!playerHasPatient)
        {
            PatientSpawner spawner = FindObjectOfType<PatientSpawner>();
            spawner.RemovePatientFromLine();
            playerHasPatient = true;
        }
    }
}
