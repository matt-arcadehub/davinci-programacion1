using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerFlight))]
public class PlayerInteractor : MonoBehaviour
{
    public PlayerFlight Flight { get; private set; }
    public bool IsDialogueOpen { get; private set; }

    [SerializeField] private InputActionReference interactAction;
    private Interactable currentInteractable;
    private Player player;
    private bool waitForInteractRelease;

    private void Awake()
    {
        Flight = GetComponent<PlayerFlight>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        if (currentInteractable == null) return;

        bool canInteract = !IsDialogueOpen && !Flight.IsFlying;

        currentInteractable.SetPromptVisible(canInteract);

        if (!canInteract || interactAction == null) return;

        if (waitForInteractRelease)
        {
            if (!interactAction.action.IsPressed())
            {
                waitForInteractRelease = false;
            }

            return;
        }

        if (interactAction.action.WasPressedThisFrame())
        {
            currentInteractable.Interact(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable.SetPromptVisible(false);
            currentInteractable.OnExitRange();
            currentInteractable = null;
        }
    }

    public void SetDialogueOpen(bool isOpen)
    {
        IsDialogueOpen = isOpen;
        player.SetControlsLocked(isOpen);

        if (isOpen)
        {
            currentInteractable?.SetPromptVisible(false);
        }
        else
        {
            waitForInteractRelease = true;
        }
    }
}