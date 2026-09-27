using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;

    void Start()
    {

    }

    void Update()
    {
        UpdateHealthText();
        UpdateEnergyText();
    }
    void UpdateHealthText()
    {
        if (healthText == null) return;

        float currentHealth = 0f;
        float maxHealth = 0f;

        if (player != null)
        {
            currentHealth = player.GetCurrentHealth();
            maxHealth = player.GetMaxHealth();
        }

        healthText.text = $"Vida: {currentHealth} / {maxHealth}";
    }

    void UpdateEnergyText()
    {
        if (energyText == null) return;

        float currentEnergy = 0f;
        float maxEnergy = 0f;

        if (player != null)
        {
            currentEnergy = player.GetCurrentEnergy();
            maxEnergy = player.GetMaxEnergy();
        }
 
        energyText.text = $"Energía: {currentEnergy:0.0} / {maxEnergy:0.0}";
    }


}
