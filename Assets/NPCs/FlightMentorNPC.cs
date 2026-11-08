using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlightMentorNPC : Interactable
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

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

    [SerializeField] private Button understoodButton;
    private PlayerInteractor activeInteractor;
    private bool unlockFlightOnConfirm;

    public override void Interact(PlayerInteractor interactor)
    {
        if (activeInteractor != null) return;

        if (dialoguePanel == null || dialogueText == null || understoodButton == null)
        {
            Debug.LogError("Faltan referencias de diálogo.");

            return;
        }

        activeInteractor = interactor;
        unlockFlightOnConfirm = !interactor.Flight.CanFly;
        dialogueText.text = unlockFlightOnConfirm ? firstDialogue : repeatDialogue;

        interactor.SetDialogueOpen(true);

        understoodButton.interactable = false;
        dialoguePanel.SetActive(true);

        StartCoroutine(FocusConfirmButton());
    }

    private IEnumerator FocusConfirmButton()
    {
        yield return null;

        if (activeInteractor == null)
        {
            yield break;
        }

        understoodButton.interactable = true;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(
                understoodButton.gameObject
            );
        }
    }

    public void ConfirmDialogue()
    {
        if (activeInteractor == null) return;

        if (unlockFlightOnConfirm)
        {
            activeInteractor.Flight.UnlockFlight();
        }

        CloseDialogue();
    }

    private void CloseDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (activeInteractor != null)
        {
            activeInteractor.SetDialogueOpen(false);
        }

        activeInteractor = null;
        unlockFlightOnConfirm = false;
    }

    public override void OnExitRange()
    {
        SetPromptVisible(false);
        CloseDialogue();
    }
}