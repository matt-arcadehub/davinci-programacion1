using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private Button newGameButton;

    [Header("Instructions Panel")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button instructionsButton;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button creditsButton;

    [Header("Footer Hints")]
    [SerializeField] private TMP_Text primaryHintsText;
    [SerializeField] private TMP_Text secondaryHintsText;
    [SerializeField] private Button backHintButton;

    private enum HintType
    {
        Select,
        Back
    }

    private void Awake()
    {
        if (mainButtons != null)
        {
            mainButtons.SetActive(true);
            SetActiveHint(HintType.Select);
        }

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    private void Start()
    {
        SelectButton(newGameButton);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Island_01");
    }

    public void OpenPrototype()
    {
        SceneManager.LoadScene("Prototype_TP1");
    }

    public void ShowInstructions()
    {
        mainButtons.SetActive(false);
        instructionsPanel.SetActive(true);

        SetActiveHint(HintType.Back);
        SelectButton(backHintButton);
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        mainButtons.SetActive(true);

        SetActiveHint(HintType.Select);
        SelectButton(instructionsButton);
    }

    public void ShowCredits()
    {
        mainButtons.SetActive(false);
        creditsPanel.SetActive(true);

        SetActiveHint(HintType.Back);
        SelectButton(backHintButton);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainButtons.SetActive(true);

        SetActiveHint(HintType.Select);
        SelectButton(creditsButton);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Salir: disponible en la build.");
#else
        Application.Quit();
#endif
    }

    private void SelectButton(Button button)
    {
        if (button == null || EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
    private void SetActiveHint(HintType hint)
    {
        bool isMainMenu = hint == HintType.Select;

        if (primaryHintsText != null)
        {
            primaryHintsText.enabled = isMainMenu;
        }

        if (secondaryHintsText != null)
        {
            secondaryHintsText.enabled = true;
            secondaryHintsText.text = isMainMenu ? "Esc / B  Salir" : "Esc / B  Volver";
        }

        if (backHintButton != null)
        {
            backHintButton.interactable = !isMainMenu;
        }
    }

    public void GoBack()
    {
        if (instructionsPanel.activeSelf)
        {
            CloseInstructions();
        }
        else if (creditsPanel.activeSelf)
        {
            CloseCredits();
        }
    }
}
