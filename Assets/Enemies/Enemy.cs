using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    private float maxHealth = 3f;
    private float currentHealth;
    private int damage = 1;

    [Header("Drop Settings")]
    [SerializeField, Range(0f, 1f)] private float dropItemChance = 0.5f;
    [SerializeField] private FeatherOfLife featherOfLifePrefabToDrop;
    [SerializeField] private EnergyOrb energyOrbPrefabToDrop;
    [SerializeField] private float minEnergyValueToDrop = 1f;
    [SerializeField] private float maxEnergyValueToDrop = 3f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        GameDebugLogger.Log(gameObject.name, $"recibió {damage} de daño.");

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;

            Die();
        }
        else
        {
            currentHealth -= damage;
        }

        UpdateHealthUI();
    }

    protected void UpdateHealthUI()
    {
        if (healthText == null)
        {
            GameDebugLogger.LogWarning(gameObject.name, "No tiene este prefab de UI.");

            return;
        }

        healthText.text = $"Life: {currentHealth}/{maxHealth}";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
        }
    }

    private void DropEnergyOrb()
    {
        if (energyOrbPrefabToDrop == null)
        {
            GameDebugLogger.LogWarning(gameObject.name, "No hay prefab para instanciar.");

            return;
        }

        float energyToRecoverValue = Random.Range(minEnergyValueToDrop, maxEnergyValueToDrop);
        energyToRecoverValue = Mathf.Round(energyToRecoverValue * 10f) / 10f;

        GameDebugLogger.Log(gameObject.name, $"dropeó {energyToRecoverValue} de energía.");
        EnergyOrb energyOrbDropped = Instantiate(energyOrbPrefabToDrop, transform.position, transform.rotation);
        energyOrbDropped.SetEnergyToRecover(energyToRecoverValue);
    }

    private void DropFeatherOfLife()
    {
        if (featherOfLifePrefabToDrop == null)
        {
            GameDebugLogger.LogWarning(gameObject.name, "No hay prefab para instanciar.");

            return;
        }

        Instantiate(featherOfLifePrefabToDrop, transform.position, transform.rotation);
    }

    private void TryToDropItem()
    {
        float randomChance = Random.value;

        if (randomChance <= dropItemChance)
        {
            GameDebugLogger.Log(gameObject.name, "dropeó ítem <b>Pluma de vida</b>.");
            DropFeatherOfLife();
        }
        else
        {
            GameDebugLogger.Log(gameObject.name, "no dropeó ningún ítem esta vez.");
        }
    }

    private void Die()
    {
        GameDebugLogger.LogSuccess(gameObject.name, "ha sido derrotado.");
        DropEnergyOrb();
        TryToDropItem();
        Destroy(gameObject);
    }
}
