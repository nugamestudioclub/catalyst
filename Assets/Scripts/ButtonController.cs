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
    
    [Header("UI Elements")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI energyPerClickText;
    
    private int _energy;
    private int _energyPerClick;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _energy = 0;
        _energyPerClick = 1;
        
        if (energyText == null) 
            Debug.LogWarning("ButtonController: No Energy Text Set.");
        else
            UpdateEnergyText();
        
        if (energyPerClickText == null) 
            Debug.LogWarning("ButtonController: No Energy Per Click Text Set.");
        else 
            UpdateEnergyPerClickText();
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
        UpdateEnergyPerClickText();
    }
    
    private void UpdateEnergyPerClickText()
    {
        if (energyPerClickText == null) return;
        
        energyPerClickText.text = "Energy Per Click: " + _energyPerClick;
    }
}
