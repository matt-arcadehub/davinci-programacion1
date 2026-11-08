using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlightMentorNPC : Interactable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Dialogue")]
    [SerializeField, TextArea(3, 6)]
    private string firstDialogue =
        "¡Bienvenido, joven fénix! Tu misión es encender " +
        "los tres braseros de esta isla. " +
        "Ahora puedes despegar y aterrizar. " +
        "Busca al siguiente guardián para aprender a usar el fuego.";

    [SerializeField, TextArea(2, 4)]
    private string repeatDialogue =
        "Recuerda: encuentra los tres braseros. " +
        "El siguiente guardián podrá enseñarte a usar el fuego.";

    public override void Interact(PlayerInteractor interactor)
    {
        if (dialogueUI == null) return;

        bool firstInteraction = !interactor.Flight.CanFly;

        string message = firstInteraction ? firstDialogue : repeatDialogue;

        dialogueUI.Show(interactor, message, () => {
            if (firstInteraction)
            {
                interactor.Flight.UnlockFlight();
            }
        });
    }

    public override void OnExitRange()
    {
        SetPromptVisible(false);
    }
}