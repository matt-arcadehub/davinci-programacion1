using UnityEngine;

public class FireMentorNPC : Interactable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [SerializeField, TextArea(3, 6)]
    private string firstDialogue =
        "Ya has aprendido a volar. Pero para encender " +
        "los tres braseros necesitarás dominar el fuego. " +
        "Ahora puedes lanzar llamaradas. ¡Pruébalo!";

    [SerializeField, TextArea(2, 4)]
    private string repeatDialogue =
        "Usa tus llamaradas para encender los tres braseros " +
        "que encontrarás por la isla.";

    public override void Interact(PlayerInteractor interactor)
    {
        if (dialogueUI == null)
        {
            Debug.LogError("FireMentorNPC: falta DialogueUI.");

            return;
        }

        Player player = interactor.GetComponent<Player>();

        if (player == null) return;

        bool firstInteraction = !player.FireUnlocked;

        string message = firstInteraction ? firstDialogue : repeatDialogue;

        dialogueUI.Show(interactor, message, () =>
        {
            if (firstInteraction)
            {
                player.UnlockFire();
            }
        });
    }

    public override void OnExitRange()
    {
        SetPromptVisible(false);
    }
}
