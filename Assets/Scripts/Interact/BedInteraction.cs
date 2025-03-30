using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sliderGO;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject ETGoHome;
    [SerializeField] private PatientMovemnt currentPatient;

    private float elapsedTime = 0f;
    [HideInInspector] public bool isPatientOnBed = false;
    [field:SerializeField] public bool isHealing { get; set; }
    [HideInInspector] public bool isNathan = false;
    //private PatientMovemnt patientMovemnt;

    private void Start()
    {
        ETGoHome = GameObject.FindGameObjectWithTag("GoodPlace");
    }

    private void Update()
    {
        if (isHealing)
        {
            if (!isNathan)
            {
                Movement movement = FindObjectOfType<Movement>();
                movement.isCuring = true;
            }
            elapsedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            sliderGO.SetActive(true);

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
                if (!isNathan)
                {
                    Movement movement = FindObjectOfType<Movement>();
                    movement.isCuring = false;
                }

                isNathan = false;
            }
        }
    }

    public void Interact(GameObject obj)
    {
        if (isHealing)
        {
            return;
        }

        if (currentPatient == null)
        {
            isPatientOnBed = false;
        }

        if (obj.CompareTag("Player"))
        {
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

                if (isPatientOnBed)
                {
                    if (meds.currentMediHeld > 0)
                    {
                        FindObjectOfType<PatientSpawner>().GetBarWithPatient(currentPatient.gameObject).GetComponent<PatienceBar>().healing = true;
                        elapsedTime = 0;
                        Debug.Log("Curing Patient");
                        meds.SubtractCurrentMedication(1);
                        isHealing = true;
                        return;
                    }
                }
            }
        }
        else
        {
            if (isPatientOnBed)
            {
                elapsedTime = 0;
                Debug.Log("Curing Patient");
                isHealing = true;
                isNathan = true;
                FindObjectOfType<PatientSpawner>().GetBarWithPatient(currentPatient.gameObject).GetComponent<PatienceBar>().healing = true;
                return;
            }
        }

        //Put funciton call here to put the patient in bed
        

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
