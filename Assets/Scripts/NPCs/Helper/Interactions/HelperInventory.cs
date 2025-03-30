using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperInventory : MonoBehaviour
{
    [SerializeField] private int medicationAmount = 1;  // Tracks the current amount of medication

    // This is just an example; make sure your actual method matches your setup.
    public int GetMedicationAmount()
    {
        return medicationAmount;
    }

    // Optionally, you could add a method to update this amount when the helper fetches medicine.
    public void AddMedicine(int amount)
    {
        medicationAmount += amount;
    }

    public void UseMedicine(int amount)
    {
        if (medicationAmount >= amount)
        {
            medicationAmount -= amount;
        }
    }
}
