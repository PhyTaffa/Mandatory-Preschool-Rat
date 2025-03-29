using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PerkSpawner : MonoBehaviour
{
    [SerializeField] private int currentRepLevel = 15;
    [SerializeField] private int maxRepLevel = 20;
    [SerializeField] private int nextUpgradeLevel = 0;
    [SerializeField] private GameObject perksUI;
    [SerializeField] private Perks perks;
    [SerializeField] private List<string> prePerkListName;
    [SerializeField] private List<int> prePerkListId;
    [SerializeField] private List<int> perkList;

    private void Start()
    {
        nextUpgradeLevel = currentRepLevel + 1;
    }

    void Update()
    {
        if (CanUpgrade())
        {
            nextUpgradeLevel++;
            SpawnPerks();
        }
    }

    private bool CanUpgrade()
    {
        if(currentRepLevel < maxRepLevel && currentRepLevel == nextUpgradeLevel)
        {
            return true;
        }
        return false;
    }

    private void SpawnPerks()
    {
        perksUI.SetActive(true);
        prePerkListName = CheckPerksName();
        prePerkListId = CheckPerksId();

        List<int> createPerkList = new List<int>(); 

        for (int i = 0; i < 3; i++)
        {
            int randomAdd = Random.Range(0, prePerkListId.Count + 1);

            bool isInside = false;

            for (int j = 0; j < prePerkListId.Count; j++)
            {
                if (randomAdd == prePerkListId[j])
                {
                    isInside = true;
                    break;
                }
            }

            if (!isInside) 
            {
                if (createPerkList.Count < 3)
                {
                    i--;
                }
            }
            else
            {
                bool isInside2 = false;

                for (int j = 0; j < createPerkList.Count; j++)
                {
                    if (randomAdd == createPerkList[j])
                    {
                        isInside2 = true;
                        break;
                    }
                }

                if (!isInside2)
                {
                    createPerkList.Add(randomAdd);
                }
                else
                {
                    i--;
                }
            }
        }

        perkList = createPerkList;
    }

    private List<string> CheckPerksName()
    {
        List<string> gambleList = new List<string>();

        int movUpg = perks.currentMovementUpgrade;
        int bedUpg = perks.currentBedUpgrade;
        int invUpg = perks.currentInvUpgrade;
        int helperUpg = perks.currentHelperUpgrade;
        int ppUpg = perks.currentPatienceUpgrade;

        if(invUpg < 3)
        {
            gambleList.Add(perks.GetPerkName(3));
        }

        if (movUpg == 0 && ppUpg == 0)
        {
            gambleList.Add(perks.GetPerkName(0));
            gambleList.Add(perks.GetPerkName(4));
        }
        else
        {
            if (movUpg > 0)
            {
                if (movUpg < 4)
                {
                    gambleList.Add(perks.GetPerkName(0));
                } 

                if (movUpg >= 3)
                {
                    gambleList.Add(perks.GetPerkName(4));
                }
            }
            else if(ppUpg > 0)
            {
                if (ppUpg < 4)
                {
                    gambleList.Add(perks.GetPerkName(4));
                }

                if (ppUpg >= 3)
                {
                    gambleList.Add(perks.GetPerkName(0));
                }
            }
        }

        if (bedUpg >= 1)
        {
            if(bedUpg < 7)
            {
                gambleList.Add(perks.GetPerkName(1));
            }

            if(helperUpg < 7)
            {
                gambleList.Add(perks.GetPerkName(2));
            }
        }
        else
        {
            gambleList.Add(perks.GetPerkName(1));
        }

        return gambleList;
    }

    private List<int> CheckPerksId()
    {
        List<int> gambleList = new List<int>();

        int movUpg = perks.currentMovementUpgrade;
        int bedUpg = perks.currentBedUpgrade;
        int invUpg = perks.currentInvUpgrade;
        int helperUpg = perks.currentHelperUpgrade;
        int ppUpg = perks.currentPatienceUpgrade;

        if (invUpg < 3)
        {
            gambleList.Add(3);
        }

        if (movUpg == 0 && ppUpg == 0)
        {
            gambleList.Add(0);
            gambleList.Add(4);
        }
        else
        {
            if (movUpg > 0)
            {
                if (movUpg < 4)
                {
                    gambleList.Add(0);
                }

                if (movUpg >= 3)
                {
                    gambleList.Add(4);
                }
            }
            else if (ppUpg > 0)
            {
                if (ppUpg < 4)
                {
                    gambleList.Add(4);
                }

                if (ppUpg >= 3)
                {
                    gambleList.Add(0);
                }
            }
        }

        if (bedUpg >= 1)
        {
            if (bedUpg < 7)
            {
                gambleList.Add(1);
            }

            if (helperUpg < 7)
            {
                gambleList.Add(2);
            }
        }
        else
        {
            gambleList.Add(1);
        }

        return gambleList;
    }
}
