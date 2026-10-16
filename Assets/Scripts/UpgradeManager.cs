using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the purchasing and processing of upgrades.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    private int _energyPerSec;
    private const float UpgradeCostMult = 1.35F;
    
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
    
    public static bool VerifyEnoughEnergy(Upgrade upgrade)
    {
        return upgrade.energyCost <= ButtonController.Instance.GetEnergy();
    }

    /// <summary>
    /// Attempts to make an upgrade using the Player's current collected energy.
    /// <p>
    /// If the player does not have enough energy, it will break early. </p>
    /// If the player has enough, it will process the upgrade and increase the subsequent price cost.
    /// </summary>
    /// <param name="upgrade">The upgrade to attempt to purchase.</param>
    public void TryUpgrade(Upgrade upgrade)
    {
        if (!VerifyEnoughEnergy(upgrade)) return;

        ProcessUpgrade(upgrade);
        ButtonController.Instance.IncrementEnergy(-upgrade.energyCost);
        
        upgrade.energyCost = (int) (upgrade.energyCost * UpgradeCostMult);
    }

    /// <summary>
    /// Process the upgrade and apply the effect.
    /// <p>
    /// If the upgrade is MouseClick type, it will increase the energy per click.</p>
    /// If the upgrade is OverTime type, it will increase the energy per second.
    /// </summary>
    /// <param name="upgrade">The upgrade to process.</param>
    /// <exception cref="ArgumentOutOfRangeException">If somehow the UpgradeType isn't one of the available.</exception>
    private void ProcessUpgrade(Upgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case Upgrade.UpgradeType.MouseClick:
                ButtonController.Instance.ModifyEnergyPerClick(upgrade.upgradeAmount);
                break;
            
            case Upgrade.UpgradeType.OverTime:
                ModifyEnergyPerSec(upgrade.upgradeAmount);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Update the number of Energy produced per second.
    /// <p>
    /// If this number has never been updated from 0, it will start the coroutine cycle to produce Energy.</p>
    /// </summary>
    /// <param name="amount">The amount to modify energyPerSec by.</param>
    private void ModifyEnergyPerSec(int amount)
    {
        var startCoroutine = _energyPerSec == 0;
        _energyPerSec += amount;
        ButtonController.Instance.UpdateEnergyPerSecText(_energyPerSec);
        
        if (startCoroutine) StartCoroutine(GenerateEnergy());
    }

    /// <summary>
    /// Runs the one-second cycle for producing Energy.
    /// </summary>
    private IEnumerator GenerateEnergy()
    {
        while (true)
        {
            ButtonController.Instance.IncrementEnergy(_energyPerSec);

            yield return new WaitForSeconds(1f);
        }
    }
}
