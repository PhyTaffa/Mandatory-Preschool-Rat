using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationReward : MonoBehaviour
{
    [SerializeField] private int currentRep = 0;
    [SerializeField] private int addRepValue = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddRep();
        }
    }

    public void AddRep()
    {
        currentRep += addRepValue;
    }
}
