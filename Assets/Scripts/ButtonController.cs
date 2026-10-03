using TMPro;
using UnityEngine;

/// <summary>
/// Controls the main button in the center of the screen.
/// <p>
/// Stores the amount of energy currently collected. On each click, increments the energy amount by the set amount.</p>
/// </summary>
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (energyText == null) Debug.LogWarning("ButtonController: No Energy Text Set.");
        
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

    public int GetEnergy()
    {
        return _energy;
    }

    public void ModifyEnergyPerClick(int amount)
    {
        _energyPerClick += amount;
    }
}
