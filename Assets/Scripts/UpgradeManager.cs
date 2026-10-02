using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public const float UpgradeCostMult = 1.35F;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryUpgrade(Upgrade upgrade)
    {
        if (!VerifyEnoughEnergy(upgrade)) return;

        ProcessUpgrade(upgrade);
        ButtonController.Instance.IncrementEnergy(-upgrade.energyCost);
        
        upgrade.energyCost = (int) (upgrade.energyCost * UpgradeCostMult);
    }

    public static bool VerifyEnoughEnergy(Upgrade upgrade)
    {
        return upgrade.energyCost <= ButtonController.Instance.GetEnergy();
    }

    private static void ProcessUpgrade(Upgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case Upgrade.UpgradeType.MouseClick:
                ButtonController.Instance.ModifyEnergyPerClick(upgrade.upgradeAmount);
                break;
            
            case Upgrade.UpgradeType.OverTime:
                Debug.Log("Do Nothing.");
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
}
