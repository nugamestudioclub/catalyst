using UnityEngine;
using UnityEngine.UI;

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
