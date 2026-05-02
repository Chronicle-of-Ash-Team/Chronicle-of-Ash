using System;
using UnityEngine;

public class MiniBossGate : MonoBehaviour
{
    [SerializeField] private BaseBoss boss;
    [SerializeField] private GameObject blockGate;

    private void Start()
    {
        if (boss == null)
        {
            Debug.LogError("MiniBoss reference is not set in the inspector.");
            return;
        }

        boss.OnBossDie += HandleBossDeath;
        boss.OnBossStart += HandleBossStart;
    }

    private void HandleBossStart()
    {
        blockGate.SetActive(true);
    }

    private void HandleBossDeath()
    {
        blockGate.SetActive(false);
    }
}
