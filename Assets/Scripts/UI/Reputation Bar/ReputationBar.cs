using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{

    public Slider slider;
    public int reputationLevel = 1;
    public int maxReputationLevel = 20;
    public int currentReputationXP = 0;
    public int reputationMaxXP = 100;


    public void SetReputation(int reputation)
    {
        currentReputationXP = currentReputationXP + reputation;
        slider.value = currentReputationXP;
    }

    public void SetMaxReputation(int maxReputation)
    {
        reputationMaxXP = maxReputation;
        slider.maxValue = reputationMaxXP;
    }

    public void NextLevel()
    {
        if (currentReputationXP >= reputationMaxXP)
        {
            reputationLevel++;

            currentReputationXP = currentReputationXP % reputationMaxXP;
            slider.value = currentReputationXP;

            slider.maxValue = reputationMaxXP;
        }
    }

    public void VictoryCondition()
    {
        if (reputationLevel >= maxReputationLevel)
        {
            //Show ending screen
        }
    }

    void Update()
    {
        NextLevel();
        VictoryCondition();
    }
}