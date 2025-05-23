using UnityEngine;

public class PatientInteraction : MonoBehaviour, IInteractable
{
    public bool playerHasPatient = false;
    public void Interact(GameObject obj)
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
