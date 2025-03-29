using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

    public Slider slider;
    public float currentPatience = 100;
    public int maxPatience = 100;

    [SerializeField] private float timeBetweenDecrease = 1f;
    [SerializeField] private float amountBetweenDecrease = 2f;

    void Start()
    {
        StartCoroutine(DecreaseOvertime());    
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

    void LosingCondition()
    {
        if(currentPatience >= 0)
        {
            //call the losing screen
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