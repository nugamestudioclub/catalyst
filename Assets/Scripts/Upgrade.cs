using UnityEngine;

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

    public void OnClick()
    {
        UpgradeManager.Instance.TryUpgrade(this);
    }
}
