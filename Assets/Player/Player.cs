using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference attackAction;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool ControlsLocked { get; private set; }

    [Header("Attributes")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxEnergy= 3;
    [SerializeField] private float currentEnergy;
    [SerializeField] private float energyRegenerationRate = 0.1f;
    [SerializeField] private float moveSpeed = 7f;
    private bool isDead;

    [Header("Attack Settings")]
    [SerializeField] private bool fireUnlocked = true;
    public bool FireUnlocked => fireUnlocked;
    [SerializeField] private GameObject attackPrefabToSpawn;
    [SerializeField] private float attackEnergyCost = 1f;

    [Header("Damage Settings")]
    [SerializeField, Min(0f)]
    private float invulnerabilityDuration = 0f;
    private float invulnerableUntil;

    [Header("Visual & Animation References")]
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shadowVisual;

    [Header("Hovering Animation")]
    [SerializeField] private float bobFrequency = 3f;
    [SerializeField] private float bobAmplitude = 0.08f;

    [Header("Shadow Setup")]
    [SerializeField] private Vector3 shadowOffset = new Vector3(0f, -0.35f, 0f);

    [Header("Flight Visual")]
    [SerializeField] private float flightVisualHeight = 0.5f;
    [SerializeField] private float flightTransitionSpeed = 2f;

    [Header("Damage Feedback")]
    [SerializeField, Range(1f, 15f)]
    private float blinkFrequency = 6f;

    [SerializeField, Range(0f, 1f)]
    private float blinkAlpha = 0.35f;

    private SpriteRenderer playerVisualRenderer;
    private Color originalVisualColor;

    private PlayerFlight playerFlight;
    private float currentVisualHeight;
    private Vector3 initialVisualPosition;
    private Vector2 animDirection = Vector2.down;

    public event System.Action Died;

    public void SetControlsLocked(bool isLocked)
    {
        ControlsLocked = isLocked;

        if (isLocked)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void UnlockFire()
    {
        fireUnlocked = true;
    }

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
        rb = GetComponent<Rigidbody2D>();
        playerFlight = GetComponent<PlayerFlight>();

        if (playerVisual != null)
        {
            playerVisualRenderer = playerVisual.GetComponent<SpriteRenderer>();

            if (playerVisualRenderer != null)
            {
                originalVisualColor = playerVisualRenderer.color;
            }
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
        }
    }

    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }

        if (attackAction != null)
        {
            attackAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }

        if (attackAction != null)
        {
            attackAction.action.Disable();
        }
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
        moveInput = ControlsLocked ? Vector2.zero : moveAction.action.ReadValue<Vector2>();

        UpdateAnimationDirection(moveInput);

        if (playerVisual != null)
        {
            float targetHeight = playerFlight != null && playerFlight.IsFlying
                ? flightVisualHeight
                : 0f;

            currentVisualHeight = Mathf.MoveTowards(
                currentVisualHeight,
                targetHeight,
                flightTransitionSpeed * Time.deltaTime
            );

            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

            playerVisual.localPosition = initialVisualPosition + Vector3.up * (currentVisualHeight + bobOffset);
        }

        if (!ControlsLocked && attackAction != null && attackAction.action.WasPressedThisFrame())
        {
            Attack();
        }

        RegenerateEnergy();
        UpdateDamageFeedback();
    }

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.ClampMagnitude(moveInput, 1f);

        rb.linearVelocity = ControlsLocked ? Vector2.zero : direction * moveSpeed;
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
        if (!fireUnlocked) return;

        if (playerFlight != null && !playerFlight.IsFlying) return;

        if (currentEnergy < attackEnergyCost)
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
        if (damage <= 0f || isDead) return;

        if (currentHealth <= 0f) return;

        if (Time.time < invulnerableUntil) return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        invulnerableUntil = Time.time + invulnerabilityDuration;

        GameDebugLogger.LogDanger(gameObject.name, $"recibió {damage} de daño.");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateDamageFeedback()
    {
        if (playerVisualRenderer == null || invulnerabilityDuration <= 0f) return;

        Color targetColor = originalVisualColor;

        if (Time.time < invulnerableUntil)
        {
            float elapsedTime = Time.time - (invulnerableUntil - invulnerabilityDuration);

            bool isDimmed = Mathf.FloorToInt(elapsedTime * blinkFrequency * 2f) % 2 == 0;

            if (isDimmed)
            {
                targetColor.a *= blinkAlpha;
            }
        }

        playerVisualRenderer.color = targetColor;
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
        if (isDead) return;

        isDead = true;

        GameDebugLogger.LogDanger(gameObject.name, "ha caído.");

        Died?.Invoke();
        Destroy(gameObject);
    }
}
