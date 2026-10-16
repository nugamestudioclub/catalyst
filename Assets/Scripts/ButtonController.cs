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
    public TextMeshProUGUI energyPerSecText;
    
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
        
        if (!energyText) 
            Debug.LogWarning("ButtonController: No Energy Text Set.");
        else
            UpdateEnergyText();
        
        if (!energyPerClickText) 
            Debug.LogWarning("ButtonController: No Energy Per Click Text Set.");
        else 
            UpdateEnergyPerClickText();
        
        if (!energyPerSecText) 
            Debug.LogWarning("ButtonController: No Energy Per Second Text Set.");
        else 
            UpdateEnergyPerSecText(0);
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

    public int GetEnergy()
    {
        return _energy;
    }

    public void ModifyEnergyPerClick(int amount)
    {
        _energyPerClick += amount;
        UpdateEnergyPerClickText();
    }
    
    private void UpdateEnergyText()
    {
        if (!energyText) return;
        
        energyText.text = _energy.ToString();
    }
    
    private void UpdateEnergyPerClickText()
    {
        if (!energyPerClickText) return;
        
        energyPerClickText.text = "Energy Per Click: " + _energyPerClick;
    }
    
    public void UpdateEnergyPerSecText(int perSec)
    {
        if (!energyPerSecText) return;
        
        energyPerSecText.text = "Energy Per Second: " + perSec;
    }
}
