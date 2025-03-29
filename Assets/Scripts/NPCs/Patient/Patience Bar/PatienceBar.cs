using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

    public Slider slider;

    public void SetPatience(int patience)
    {
        slider.value = patience;
    }

    public void SetMaxReputation(int maxPatience)
    {
        slider.maxValue = maxPatience;
    }
}