using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlight : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference toggleFlightAction;

    [Header("Landing Settings")]
    [SerializeField] private LayerMask landingZoneMask;
    [SerializeField] private LayerMask groundObstacleMask;
    [SerializeField] private float landingCheckRadius = 0.4f;

    [Header("Visual Sorting")]
    [SerializeField] private SpriteRenderer playerVisualRenderer;

    [SerializeField] private string groundSortingLayer = "Default";
    [SerializeField] private string airSortingLayer = "AirActors";

    private Player player;
    private int groundLayer;
    private int airLayer;

    public bool CanFly { get; private set; } = false;
    public bool IsFlying { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        groundLayer = LayerMask.NameToLayer("PlayerGround");
        airLayer = LayerMask.NameToLayer("PlayerAir");

        if (groundLayer < 0 || airLayer < 0)
        {
            Debug.LogError("PlayerFlight: faltan Physics Layers.");
            enabled = false;

            return;
        }

        IsFlying = false;
        gameObject.layer = groundLayer;

        UpdateVisualSorting();
    }

    private void OnEnable()
    {
        if (toggleFlightAction != null)
        {
            toggleFlightAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleFlightAction != null)
        {
            toggleFlightAction.action.Disable();
        }
    }

    private void Update()
    {
        if (player.ControlsLocked) return;

        if (toggleFlightAction == null || !toggleFlightAction.action.WasPressedThisFrame()) return;

        if (!IsFlying)
        {
            TakeOff();
        }
        else
        {
            TryLand();
        }
    }

    public void UnlockFlight()
    {
        CanFly = true;
    }

    private void UpdateVisualSorting()
    {
        if (playerVisualRenderer == null)
            return;

        playerVisualRenderer.sortingLayerName =
            IsFlying ? airSortingLayer : groundSortingLayer;

        playerVisualRenderer.sortingOrder =
            IsFlying ? 0 : 1;
    }

    private void TakeOff()
    {
        if (!CanFly) return;

        IsFlying = true;
        gameObject.layer = airLayer;

        UpdateVisualSorting();
    }

    private void TryLand()
    {
        Vector2 position = transform.position;

        bool hasGround = Physics2D.OverlapCircle(
            position,
            landingCheckRadius,
            landingZoneMask
        ) != null;

        bool isBlocked = Physics2D.OverlapCircle(
            position,
            landingCheckRadius,
            groundObstacleMask
        ) != null;

        if (!hasGround || isBlocked)
        {
            Debug.Log("No es posible aterrizar aquí.");

            return;
        }

        IsFlying = false;
        gameObject.layer = groundLayer;

        UpdateVisualSorting();
    }
}