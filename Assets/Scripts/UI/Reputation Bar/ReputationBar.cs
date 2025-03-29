using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{

    public Slider slider;
    public int reputationLevel = 1;
    public int maxReputationLevel = 20;

    public void SetReputation(int reputation)
    {
        slider.value = reputation;
    }

    public void SetMaxReputation(int maxReputation)
    {
        slider.maxValue = maxReputation;
    }

    public void NextLevel(int maxReputation)
    {
        if (slider.value >= slider.maxValue)
        {
            reputationLevel++;
            slider.value = 0;
            slider.maxValue = maxReputation;
        }
    }

    public void VictoryCondition()
    {
        if (reputationLevel >= maxReputationLevel)
        {
            //Show ending screen
        }
    }
}