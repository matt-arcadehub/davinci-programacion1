using UnityEngine;

public class DefeatUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject defeatPanel;

    private void Awake()
    {
        if (defeatPanel != null) {
            defeatPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.Died += ShowDefeat;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.Died -= ShowDefeat;
        }
    }

    private void ShowDefeat()
    {
        if (dialogueUI != null)
        {
            dialogueUI.Close();
        }

        if (defeatPanel == null) return;

        defeatPanel.SetActive(true);
    }
}
