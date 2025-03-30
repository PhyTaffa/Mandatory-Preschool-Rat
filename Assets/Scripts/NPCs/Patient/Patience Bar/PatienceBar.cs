using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

    public Slider slider;
    public float currentPatience = 100;
    public float maxPatience = 100;
    public bool isDead = false;
    private GameObject murderMe;

    [SerializeField] private float timeBetweenDecrease = 1f;
    [SerializeField] private float amountBetweenDecrease = 2f;

    void Start()
    {
        StartCoroutine(DecreaseOvertime());
        murderMe = FindObjectOfType<PatientSpawner>().gameObject;
    }

    void Update()
    {
        LostPatientCondition();
    }

    private void SubtractPatience(float patience)
    {
        currentPatience = currentPatience - patience;
        slider.value = currentPatience;
    }

    public void SetMaxPatience(float num)
    {
        maxPatience = num;
        slider.maxValue = maxPatience;
    }

    

    void LostPatientCondition()
    {
        if(currentPatience <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                murderMe.GetComponent<PatientSpawner>().murderPatient.Invoke(gameObject);
                StopAllCoroutines();
            }
            
    
        }
    }

    private IEnumerator DecreaseOvertime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenDecrease);
            SubtractPatience(amountBetweenDecrease);
        }
    }
}