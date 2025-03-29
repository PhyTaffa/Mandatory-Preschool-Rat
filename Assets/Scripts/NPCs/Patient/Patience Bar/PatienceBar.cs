using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

    public Slider slider;
    public float currentPatience = 100;
    public int maxPatience = 100;
    public bool isDead = false;

    [SerializeField] private float timeBetweenDecrease = 1f;
    [SerializeField] private float amountBetweenDecrease = 2f;

    void Start()
    {
        StartCoroutine(DecreaseOvertime());    
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

    private void SetMaxPatience()
    {
        slider.maxValue = maxPatience;
    }

    

    void LostPatientCondition()
    {
        if(currentPatience <= 0)
        {
            if (!isDead)
            {
                isDead = true;
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