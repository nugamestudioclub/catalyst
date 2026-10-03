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

    private Button _button;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        if (_button) _button.interactable = UpgradeManager.VerifyEnoughEnergy(this);
    }

    public void OnClick()
    {
        UpgradeManager.Instance.TryUpgrade(this);
    }
}
