using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

    public Slider slider;
    public int currentPatience = 100;
    public int maxPatience = 100;

    private float newPatienceTimer;
    private float patienceTimerLimit = 50f;

    void patienceInit()
    {
        newPatienceTimer = patienceTimerLimit;
    }

    private void SubtractPatience(int patience)
    {
        currentPatience = currentPatience - patience;
        slider.value = currentPatience;
    }

    private void SetMaxPatience()
    {
        slider.maxValue = maxPatience;
    }

    void Update()
    {
        
    }
}