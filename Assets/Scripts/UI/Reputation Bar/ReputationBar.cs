using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReputationBar : MonoBehaviour
{

    public Slider slider;
    [SerializeField] private int reputationLevel = 1;
    [SerializeField] private int maxReputationLevel = 20;
    [SerializeField] private float currentReputationXP = 0;
    [SerializeField] private int reputationMaxXP = 100;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject victory;


    void SetReputation(int reputation)
    {
        currentReputationXP = currentReputationXP + reputation;
        slider.value = currentReputationXP;
    }

    void SetMaxReputation(int maxReputation)
    {
        reputationMaxXP = maxReputation;
        slider.maxValue = reputationMaxXP;
    }

    void NextLevel()
    {
        if (currentReputationXP >= reputationMaxXP)
        {
            reputationLevel++;

            currentReputationXP = currentReputationXP % reputationMaxXP;
            slider.value = currentReputationXP;

            slider.maxValue = reputationMaxXP;
        }
    }

    void VictoryCondition()
    {
        if (reputationLevel >= maxReputationLevel)
        {
            //Show victory screen
            victory.SetActive(true);
        }
    }

    void Update()
    {
        NextLevel();
        VictoryCondition();

        text.text = reputationLevel.ToString();
    }
}