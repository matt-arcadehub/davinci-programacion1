using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefeatUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Button retryButton;

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
        // Cerrar cualquier conversación que pudiera estar abierta.
        if (dialogueUI != null)
        {
            dialogueUI.Close();
        }

        if (defeatPanel == null) return;

        defeatPanel.SetActive(true);

        if (retryButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(retryButton.gameObject);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
