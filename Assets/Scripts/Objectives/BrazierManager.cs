using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BrazierManager : MonoBehaviour
{
    [Header("Mission References")]
    [SerializeField] private Brazier[] braziers;
    [SerializeField] private Portal portal;

    [Header("UI")]
    [SerializeField] private TMP_Text progressText;

    [Header("Progress Visuals")]
    [SerializeField] private Color completedColor = Color.yellow;

    private Color normalColor;

    private void OnEnable()
    {
        foreach (Brazier brazier in braziers)
        {
            if (brazier != null)
            {
                brazier.Lit += OnBrazierLit;
            }
        }
    }

    private void Awake()
    {
        if (progressText == null) return;

        normalColor = progressText.color;
        progressText.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateProgress();
    }

    private void OnDisable()
    {
        foreach (Brazier brazier in braziers)
        {
            if (brazier != null)
            {
                brazier.Lit -= OnBrazierLit;
            }
        }
    }
    public void ShowObjective()
    {
        if (progressText == null) return;

        progressText.gameObject.SetActive(true);
        UpdateProgress();
    }

    private void OnBrazierLit(Brazier brazier)
    {
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        int litCount = 0;
        int totalCount = 0;

        foreach (Brazier brazier in braziers)
        {
            if (brazier == null) continue;

            totalCount++;

            if (brazier.IsLit)
            {
                litCount++;
            }
        }

        if (progressText != null)
        {
            progressText.text = $"Braseros: {litCount}/{totalCount}";

            bool objectiveCompleted = totalCount > 0 && litCount == totalCount;

            progressText.color = objectiveCompleted ? completedColor : normalColor;
        }

        if (totalCount > 0 && litCount == totalCount)
        {
            if (portal != null)
            {
                portal.Activate();
            }
        }
    }
}
