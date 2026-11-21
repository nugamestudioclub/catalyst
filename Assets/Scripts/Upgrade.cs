using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Placed on Upgrade buttons.
/// <p>
/// Specifies all the details of the upgrade, including energy cost, upgrade type,
/// and the upgrade amount. </p>
/// Relies on UpgradeManager to complete purchases.
/// </summary>
public class Upgrade : MonoBehaviour
{
    public enum UpgradeType
    {
        MouseClick,
        OverTime
    }

    public int energyCost;
    public UpgradeType upgradeType;
    public int upgradeAmount;

    [Tooltip("Shows the effect and current cost.")]
    public TextMeshProUGUI label;

    private Button _button;
    private int _displayedCost = -1;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (_button) _button.interactable = UpgradeManager.VerifyEnoughEnergy(this);
        if (label && _displayedCost != energyCost) UpdateLabel();
    }

    public void OnClick()
    {
        UpgradeManager.Instance.TryUpgrade(this);
    }

    private void UpdateLabel()
    {
        string effect = upgradeType switch
        {
            UpgradeType.MouseClick => "Energy Per Click",
            UpgradeType.OverTime => "Energy Per Second",
            _ => throw new ArgumentOutOfRangeException()
        };

        label.text = $"> +{upgradeAmount} {effect}\n  Cost: {energyCost}";
        _displayedCost = energyCost;
    }
}
