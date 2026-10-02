using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public static ButtonController Instance;
    
    [Header("Energy Tracking")]
    public TextMeshProUGUI energyText;
    
    private int _energy;
    private int _energyPerClick;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (energyText == null) Debug.LogWarning("ButtonController: No Energy Text Set.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _energy = 0;
        _energyPerClick = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void OnClick()
    {
        IncrementEnergy(_energyPerClick);
    }

    public void IncrementEnergy(int amount)
    {
        _energy += amount;
        UpdateEnergyText();
    }

    private void UpdateEnergyText()
    {
        if (energyText == null) return;
        
        energyText.text = _energy.ToString();
    }
}
