using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationReward : MonoBehaviour
{
    [SerializeField] private ReputationBar reputation;
    [SerializeField] private PatientSpawner prefab;
    [SerializeField] private int addRepValue = 10;

    public void AddRep(float patience)
    {
        reputation.SetReputation(addRepValue * (patience / prefab.GetMaxPatienceBar()));
    }
}