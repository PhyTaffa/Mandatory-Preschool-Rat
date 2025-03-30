using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private ReputationBar repBar;

    private void Start()
    {
        currentRepLevel = repBar.reputationLevel;
        nextUpgradeLevel = currentRepLevel + 1;
    }

    void Update()
    {
        currentRepLevel = repBar.reputationLevel;

        if (CanUpgrade())
        {
            nextUpgradeLevel++;
            gameStateManager.paused = true;
            SpawnPerks();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            repBar.SetReputation(50);
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

        if(prePerkListId.Count <= 3)
        {
            createPerkList = prePerkListId;
        }
        else if (prePerkListId.Count > 3)
        {
            int attempts = 0;

            for (int i = 0; i < 3; i++)
            {
                attempts++;

                if (attempts >= 100)
                {
                    break;
                }

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
        }

        perkList = createPerkList;

        if (perkList.Count == 3)
        {
            buttons[0].gameObject.SetActive(true);

            Button one = buttons[0].gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            Button two = buttons[0].gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
            Button three = buttons[0].gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();

            one.onClick.AddListener(() => perks.GetPerkFunction(perkList[0]));
            one.image.sprite = perks.GetPerkImage(perkList[0]);
            one.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[0]);

            two.onClick.AddListener(() => perks.GetPerkFunction(perkList[1]));
            two.image.sprite = perks.GetPerkImage(perkList[1]);
            two.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[1]);

            three.onClick.AddListener(() => perks.GetPerkFunction(perkList[2]));
            three.image.sprite = perks.GetPerkImage(perkList[2]);
            three.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[2]);
        }
        else if (perkList.Count == 2)
        {
            buttons[1].gameObject.SetActive(true);
            
            Button one = buttons[1].gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            Button two = buttons[1].gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();

            one.onClick.AddListener(() => perks.GetPerkFunction(perkList[0]));
            one.image.sprite = perks.GetPerkImage(perkList[0]);
            one.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[0]);

            two.onClick.AddListener(() => perks.GetPerkFunction(perkList[1]));
            two.image.sprite = perks.GetPerkImage(perkList[1]);
            two.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[1]);
        }
        else if (perkList.Count == 1)
        {
            buttons[2].gameObject.SetActive(true);
            
            Button one = buttons[2].gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();

            one.onClick.AddListener(() => perks.GetPerkFunction(perkList[0]));
            one.image.sprite = perks.GetPerkImage(perkList[0]);
            one.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = perks.GetPerkName(perkList[0]);
        }
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

                if (movUpg >= 3 && ppUpg < 4)
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

                if (ppUpg >= 3 && movUpg < 4)
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

            if(helperUpg < 7 && helperUpg < bedUpg)
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

                if (movUpg >= 3 && ppUpg < 4)
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

                if (ppUpg >= 3 && movUpg < 4)
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

            if (helperUpg < 7 && helperUpg < bedUpg)
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

    public void TakeOffListenners()
    {
        gameStateManager.paused = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);

            for (int j = 0; j < buttons[i].gameObject.transform.childCount; j++)
            {
                buttons[i].gameObject.transform.GetChild(j).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
        perksUI.gameObject.SetActive(false);
    }
}
