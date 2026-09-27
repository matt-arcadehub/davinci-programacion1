using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference attackAction;

    [Header("Attributes")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxEnergy= 3;
    [SerializeField] private float currentEnergy;
    [SerializeField] private float energyRegenerationRate = 0.1f;
    [SerializeField] private float moveSpeed = 7f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackPrefabToSpawn;
    [SerializeField] private float attackEnergyCost = 1f;

    [Header("Visual & Animation References")]
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shadowVisual;

    [Header("Hovering Animation")]
    [SerializeField] private float bobFrequency = 3f;
    [SerializeField] private float bobAmplitude = 0.08f;

    [Header("Shadow Setup")]
    [SerializeField] private Vector3 shadowOffset = new Vector3(0f, -0.6f, 0f);

    private Vector3 initialVisualPosition;
    private Vector2 animDirection = Vector2.down;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Start()
    {
        if (playerVisual != null)
        {
            initialVisualPosition = playerVisual.localPosition;
        }

        if (shadowVisual != null)
        {
            shadowVisual.localPosition = shadowOffset;
        }

        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, input.y, 0f).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        UpdateAnimationDirection(input);

        if (playerVisual != null)
        {
            float newY = initialVisualPosition.y + (Mathf.Sin(Time.time * bobFrequency) * bobAmplitude);
            playerVisual.localPosition = new Vector3(initialVisualPosition.x, newY, initialVisualPosition.z);
        }

        if (attackAction != null && attackAction.action.WasPressedThisFrame())
        {
            Attack();
        }

        RegenerateEnergy();
    }

    private void UpdateAnimationDirection(Vector2 input)
    {
        if (animator == null) return;

        bool isXActive = Mathf.Abs(input.x) > 0.01f;
        bool isYActive = Mathf.Abs(input.y) > 0.01f;

        if (isXActive && isYActive)
        {
            if (animDirection.x != 0)
            {
                animDirection = new Vector2(Mathf.Sign(input.x), 0f);
            }
            else if (animDirection.y != 0)
            {
                animDirection = new Vector2(0f, Mathf.Sign(input.y));
            }
        }
        else if (isYActive)
        {
            animDirection = new Vector2(0f, Mathf.Sign(input.y));
        }
        else if (isXActive)
        {
            animDirection = new Vector2(Mathf.Sign(input.x), 0f);
        }

        bool isMoving = isXActive || isYActive;

        animator.SetFloat("MoveX", animDirection.x);
        animator.SetFloat("MoveY", animDirection.y);
        animator.SetBool("IsMoving", isMoving);
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy >= maxEnergy) return;

        float energyToRegenerate = energyRegenerationRate * Time.deltaTime;

        if (currentEnergy + energyToRegenerate > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else
        {
            currentEnergy += energyToRegenerate;
        }
    }

    private void Attack()
    {
        if (currentEnergy - attackEnergyCost <= 0)
        {
            GameDebugLogger.LogWarning(gameObject.name, "no tiene suficiente energía para realizar esta acción.");

            return;
        }
        else if (attackPrefabToSpawn == null)
        {
            GameDebugLogger.LogWarning(gameObject.name, "No hay prefab para instanciar.");

            return;
        }

        currentEnergy -= attackEnergyCost;

        float angle = Mathf.Atan2(-animDirection.x, animDirection.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        Instantiate(attackPrefabToSpawn, transform.position, rotation);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;

        GameDebugLogger.LogDanger(gameObject.name, $"recibió {damage} de daño.");

        if (currentHealth - damage <= 0f)
        {
            currentHealth = 0f;

            Die();
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public void CollectEnergyOrb(float energyOrbAmount)
    {
        if (energyOrbAmount <= 0f) return;

        if (currentEnergy + energyOrbAmount >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else
        {
            currentEnergy += energyOrbAmount;
        }

        GameDebugLogger.LogInfo(gameObject.name, $"<b>Orbe de energía</b> colectado: +{energyOrbAmount} de energía.");
    }

    public void CollectFeatherOfLife()
    {
        if (currentHealth == maxHealth)
        {
            GameDebugLogger.LogInfo(gameObject.name, "Ítem <b>Pluma de vida</b> colectado: máximo de vida alcanzado.");

            return;
        }

        GameDebugLogger.LogInfo(gameObject.name, "Ítem <b>Pluma de vida</b> colectado: +1 de vida.");
        currentHealth++;
    }

    private void Die()
    {
        GameDebugLogger.LogDanger(gameObject.name, "ha caído.");
        Destroy(gameObject);
    }
}
