using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private Button newGameButton;

    [Header("Instructions Panel")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button instructionsBackButton;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button creditsBackButton;

    private void Awake()
    {
        if (mainButtons != null)
        {
            mainButtons.SetActive(true);
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

        SelectButton(instructionsBackButton);
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        mainButtons.SetActive(true);

        SelectButton(instructionsButton);
    }

    public void ShowCredits()
    {
        mainButtons.SetActive(false);
        creditsPanel.SetActive(true);

        SelectButton(creditsBackButton);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainButtons.SetActive(true);

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
}
