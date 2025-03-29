using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Perks : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Movement Perk")]
    [SerializeField] private int maxMovementUpgrade = 3;
    [field:SerializeField] public int currentMovementUpgrade { get; private set; }
    [SerializeField] private int movementUpgrade = 2;

    [Header("Bed Perk")]
    [SerializeField] private int maxBedUpgrade = 7;
    [field: SerializeField] public int currentBedUpgrade { get; private set; }
    [SerializeField] private GameObject bedPrefab;
    [SerializeField] private GameObject bedParent;

    [Header("Helper Perk")]
    [SerializeField] private int maxHelperUpgrade = 7;
    [field: SerializeField] public int currentHelperUpgrade { get; private set; }
    [SerializeField] private GameObject helperPrefab;
    [SerializeField] private GameObject helperParent;

    [Header("Medicine Inv Perk")]
    [SerializeField] private int maxInvUpgrade = 3;
    [field: SerializeField] public int currentInvUpgrade { get; private set; }
    [SerializeField] private int invSlot = 1;

    [Header("Patients Patience Perk")]
    [SerializeField] private int maxPatienceUpgrade = 3;
    [field: SerializeField] public int currentPatienceUpgrade { get; private set; }
    [SerializeField] private int patienceUpgrade = 25;
    [SerializeField] private float patienceLevel = 100;

    [Header("Perk")]
    [SerializeField] private string[] perkName;
    [SerializeField] private Sprite[] perkImage;

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
        if (Input.GetKeyDown(KeyCode.B))
        {
            UpgradeBeds();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UpgradeHelpers();
        }
    }

    public void UpgradeMovement()
    {
        currentMovementUpgrade++;
        float speed = player.GetComponent<Movement>().getSpeed() + movementUpgrade * Mathf.Sqrt(currentMovementUpgrade);
        player.GetComponent<Movement>().SetSpeed(speed);
    }

    public void UpgradeBeds()
    {
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
        if(beds.Length == 4)
        {
            Instantiate(bedPrefab, beds[0].transform.position + new Vector3(0, -8, 0), Quaternion.identity, bedParent.transform);
        }
        else
        {
            Instantiate(bedPrefab, beds[beds.Length-1].transform.position + new Vector3(4, 0, 0), Quaternion.identity, bedParent.transform);   
        }
        currentBedUpgrade++;
    }
    public void UpgradeHelpers()
    {
        Instantiate(helperPrefab, helperParent.transform.position, Quaternion.identity, helperParent.transform);
        currentHelperUpgrade++;
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
    public Sprite GetPerkImage(int perkNum)
    {
        for (int i = 0; i < 5; i++)
        {
            if (perkNum == i)
            {
                return perkImage[i];
            }
        }
        return null;
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
