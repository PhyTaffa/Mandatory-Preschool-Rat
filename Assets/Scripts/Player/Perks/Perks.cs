using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Perks : MonoBehaviour
{
    [Header("Movement Perk")]
    [SerializeField] private int maxMovementUpgrade = 3;
    [field:SerializeField] public int currentMovementUpgrade { get; private set; }
    [SerializeField] private int movementUpgrade = 2;
    [SerializeField] private float movement = 6;

    [Header("Bed Perk")]
    [SerializeField] private int maxBedUpgrade = 7;
    [field: SerializeField] public int currentBedUpgrade { get; private set; }

    [Header("Helper Perk")]
    [SerializeField] private int maxHelperUpgrade = 7;
    [field: SerializeField] public int currentHelperUpgrade { get; private set; }

    [Header("Medicine Inv Perk")]
    [SerializeField] private int maxInvUpgrade = 3;
    [field: SerializeField] public int currentInvUpgrade { get; private set; }
    [SerializeField] private int invSlot = 1;

    [Header("Patients Patience Perk")]
    [SerializeField] private int maxPatienceUpgrade = 3;
    [field: SerializeField] public int currentPatienceUpgrade { get; private set; }
    [SerializeField] private int patienceUpgrade = 25;
    [SerializeField] private float patienceLevel = 100;

    [Header("Perk Names")]
    [SerializeField] private string[] perkName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpgradeMovement();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            UpgradeInv();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            UpgradePatience();
        }
    }

    public void UpgradeMovement()
    {
        currentMovementUpgrade++;
        movement += (movementUpgrade * Mathf.Sqrt(currentMovementUpgrade));
    }

    public void UpgradeBeds()
    {
        //Diogo
    }
    public void UpgradeHelpers()
    {
        //Diogo
    }


    public void UpgradeInv()
    {
        invSlot++;
        currentInvUpgrade++;
    }

    public void UpgradePatience()
    {
        currentPatienceUpgrade++;
        patienceLevel = 100 + (patienceUpgrade * Mathf.Sqrt(currentPatienceUpgrade));
    }

    public string GetPerkName(int perkNum)
    {
        for (int i = 0; i < 5; i++)
        {
            if (perkNum == i)
            {
                return perkName[i];
            }
        }
        return "No perk name";
    }

    public void GetPerkFunction(int perkNum)
    {
        if (perkNum == 0)
        {
            UpgradeMovement();
        }
        else if (perkNum == 1)
        {
            UpgradeBeds();
        }
        else if (perkNum == 2)
        {
            UpgradeHelpers();
        }
        else if (perkNum == 3)
        {
            UpgradeInv();
        }
        else if (perkNum == 4)
        {
            UpgradePatience();
        }
    }
}
