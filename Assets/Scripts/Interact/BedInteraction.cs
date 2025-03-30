using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sliderGO;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject ETGoHome;

    private float elapsedTime = 0f;
    private bool isPatientOnBed = false;
    private bool a = false;
    //private PatientMovemnt patientMovemnt;

    private void Update()
    {
        if (a)
        {
            elapsedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Update the slider value to reflect the elapsed time
            slider.value = elapsedTime;

            if (seconds == 3)
            {
                Debug.Log("Cured");
                a = false;
                sliderGO.SetActive(false);
                PatientMovemnt patientMovemnt = patientMovemnt = FindObjectOfType<PatientMovemnt>();
                patientMovemnt = FindObjectOfType<PatientMovemnt>();
                patientMovemnt.SendPatientToHome(ETGoHome);
            }
        }
    }

    public void Interact()
    {
        //Put funciton call here to put the patient in bed
        Debug.Log("Interacted with the a Bed");

        PatientInteraction patientInteraction = FindObjectOfType<PatientInteraction>();
        Medication meds = FindObjectOfType<Medication>();
        PatientMovemnt patientMovemnt = patientMovemnt = FindObjectOfType<PatientMovemnt>();

        if (isPatientOnBed && meds.currentMediHeld > 0)
        {
            Debug.Log("Curring Patient");

            sliderGO.SetActive(true);
            meds.SubtractCurrentMedication(1);
            a = true;

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
