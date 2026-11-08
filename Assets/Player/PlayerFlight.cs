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

    private int groundLayer;
    private int airLayer;

    public bool IsFlying { get; private set; }

    private void Awake()
    {
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

    private void TakeOff()
    {
        IsFlying = true;
        gameObject.layer = airLayer;
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
    }
}