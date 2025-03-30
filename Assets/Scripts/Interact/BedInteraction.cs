using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sliderGO;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject ETGoHome;
    [SerializeField] private PatientMovemnt currentPatient;

    private float elapsedTime = 0f;
    private bool isPatientOnBed = false;
    private bool isHealing = false;
    //private PatientMovemnt patientMovemnt;

    private void Update()
    {
        if (isHealing)
        {
            Movement movement = FindObjectOfType<Movement>();
            movement.isCuring = true;
            elapsedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Update the slider value to reflect the elapsed time
            slider.value = elapsedTime;

            if (seconds == 3)
            {
                Debug.Log("Cured");
                isHealing = false;
                sliderGO.SetActive(false);

                isPatientOnBed = false;

                //PatientMovemnt patientMovemnt = patientMovemnt = FindObjectOfType<PatientMovemnt>();
                //patientMovemnt = FindObjectOfType<PatientMovemnt>();
                currentPatient.SendPatientToHome(ETGoHome);

                currentPatient = null;
                movement.isCuring = false;
            }
        }
    }

    public void Interact()
    {
        //Put funciton call here to put the patient in bed
        if (currentPatient == null)
        {
            isPatientOnBed = false;
        }

        if (!isPatientOnBed)
        {
            foreach (PatientMovemnt patient in FindObjectsOfType<PatientMovemnt>())
            {
                if (patient.patientState == 2)
                {
                    patient.MoveToBed(gameObject);
                    currentPatient = patient;
                    isPatientOnBed = true;
                    FindObjectOfType<PatientInteraction>().playerHasPatient = false;
                }
            }
        }
        else
        {
            Medication meds = FindObjectOfType<Medication>();

            if (isPatientOnBed && meds.currentMediHeld > 0)
            {
                elapsedTime = 0;
                Debug.Log("Curing Patient");
                FindObjectOfType<PatientSpawner>().GetBarWithPatient(currentPatient.gameObject).GetComponent<PatienceBar>().healing = true;
                sliderGO.SetActive(true);
                meds.SubtractCurrentMedication(1);
                isHealing = true;
            }
        }

        //Debug.Log("Interacted with the a Bed");

        //PatientInteraction patientInteraction = FindObjectOfType<PatientInteraction>();
        //PatientMovemnt patientMovemnt = patientMovemnt = FindObjectOfType<PatientMovemnt>();

        //Debug.Log(patientInteraction.playerHasPatient);

        //if (patientInteraction.playerHasPatient)
        //{
        //    Debug.Log("Patient is on bed");

        //    isPatientOnBed = true;

        //    patientInteraction.playerHasPatient = false;
        //    patientMovemnt.MoveToBed(gameObject);
        //}
    }
}
