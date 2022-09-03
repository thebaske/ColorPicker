using System;
using DataSystem;
using UnityEngine;
using Upgradables;
using Random = UnityEngine.Random;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private GameEvent OnTriggerSave;
    [SerializeField] private UpgradeData[] upgradables;
    private Upgradables_UI upgradablesUI;
    [Header("Reset this one")] [SerializeField] UpgradeData[] udtoRest;
    private void Awake()
    {
        upgradablesUI = FindObjectOfType<Upgradables_UI>();
        for (int i = 0; i < udtoRest.Length; i++)
        {
            udtoRest[i].upgradable.ResetMyData();
        }
        ShowUpgradables();
        for (int i = 0; i < upgradables.Length; i++)
        {
            upgradables[i].upgradable.OnUpgradeSuccessfull += TriggerSave;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < upgradables.Length; i++)
        {
            upgradables[i].upgradable.OnUpgradeSuccessfull -= TriggerSave;
        }
    }

    public void ShowUpgradables()
    {
        UpgradeData[] tmp = new UpgradeData[3];
        UpgradeData[] tmpShuffle = Utility.ShuffleArray(upgradables, Random.Range(0, 888));
        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i] = tmpShuffle[i];
        }
        upgradablesUI.ReceiveUpgradables(tmp);
        
    }
    void CheckIfMaxedOut()
    {
        for (int i = 0; i < upgradables.Length; i++)
        {
            upgradablesUI.Refresh();
            // if (!upgradables[i].upgradable.CanUpgrade())
            // {
            //     //handle maxed out;
            //     upgradables[i].Name = "MAXED OUT";
            //     
            //     
            // }
        }
        
    }
    void TriggerSave()
    {
        OnTriggerSave.RaiseEmpty();
        CheckIfMaxedOut();
    }
}

[System.Serializable]
public class UpgradeData
{
    public UpgradableValueBase upgradable;
    public Sprite image;
    public string Name;
}
