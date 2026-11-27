using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Player player;

    [Header("HUD")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private Image[] healthFeathers;
    [SerializeField] private Image energyFill;

    private void OnEnable()
    {
        if (player != null)
        {
            player.Died += HideHUD;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.Died -= HideHUD;
        }
    }

    private void HideHUD()
    {
        if (playerHUD != null)
        {
            playerHUD.SetActive(false);
        }
    }

    void Update()
    {
        UpdateHealth();
        UpdateEnergy();
    }

    private void UpdateHealth()
    {
        if (player == null) return;

        float currentHealth = player.GetCurrentHealth();

        for (int i = 0; i < healthFeathers.Length; i++)
        {
            Image feather = healthFeathers[i];

            if (feather == null) continue;

            Color color = feather.color;
            color.a = currentHealth >= i + 1 ? 1f : 0.25f;

            feather.color = color;
        }
    }

    private void UpdateEnergy()
    {
        if (player == null || energyFill == null) return;

        float maxEnergy = player.GetMaxEnergy();

        energyFill.fillAmount = maxEnergy > 0f ? Mathf.Clamp01(player.GetCurrentEnergy() / maxEnergy) : 0f;
    }
}
