using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Medication : MonoBehaviour
{
    public int currentMediHeld = 0;
    [SerializeField] private int maxMediHeld = 1;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = currentMediHeld.ToString() + " / " + maxMediHeld.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = currentMediHeld.ToString() + " / " + maxMediHeld.ToString();
    }

    public void AddMaxMedication(int maxMedication)
    {
        maxMediHeld = maxMediHeld + maxMedication;
    }

    public void AddCurrentMedication(int amountOfMedication)
    {
        if (currentMediHeld + amountOfMedication <= maxMediHeld)
        {
            currentMediHeld = currentMediHeld + amountOfMedication;
        }
    }

    public void SubtractCurrentMedication(int amountOfMedication)
    {
        currentMediHeld = currentMediHeld - amountOfMedication;
    }

}
