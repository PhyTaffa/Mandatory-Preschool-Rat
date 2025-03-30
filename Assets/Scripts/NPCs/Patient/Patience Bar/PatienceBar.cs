using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    public Slider slider;
    public float currentPatience = 100;
    public float maxPatience = 100;
    public bool isDead = false;
    public bool healing = false;
    public GameStateManager paused;
    private GameObject murderMe;

    [SerializeField] private float timeBetweenDecrease = 1f;
    [SerializeField] private float amountBetweenDecrease = 2f;

    void Start()
    {
        paused = FindObjectOfType<GameStateManager>();
        StartCoroutine(DecreaseOvertime());
        murderMe = FindObjectOfType<PatientSpawner>().gameObject;
        currentPatience = maxPatience;
    }

    void Update()
    {
        if(!isDead)
        {
            LostPatientCondition();
        }
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

    public float GetPatience()
    {
        return currentPatience;
    }

    public void StopPatientBarTimer()
    {
        StopAllCoroutines();
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
        while (!healing)
        {
            if (!paused.paused)
            {
                yield return new WaitForSeconds(timeBetweenDecrease);
                SubtractPatience(amountBetweenDecrease);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}