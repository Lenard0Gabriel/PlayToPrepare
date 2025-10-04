using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StormMiniGame : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;    // must be size 3
        public int correctIndex;    // which answer is correct
    }

    [Header("Mini Game Settings")]
    public Question[] questions;
    private int currentQuestion = 0;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;

    [Header("Timer Settings")]
    public float timeLimit = 30f; // total time in seconds
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;

    [Header("Manager Reference")]
    public MegaScenarioManager scenarioManager;

    private bool gameActive = false;

    private void OnEnable()
    {
        currentQuestion = 0;
        LoadQuestion();

        // Reset timer
        timeRemaining = timeLimit;
        gameActive = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    private void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
            timeRemaining = 0;

        if (timerText != null)
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString(); // only the number

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    void LoadQuestion()
    {
        feedbackText.text = "";
        Question q = questions[currentQuestion];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    void OnAnswerSelected(int index)
    {
        if (index == questions[currentQuestion].correctIndex)
        {
            feedbackText.text = "Correct!";
            Invoke(nameof(NextStep), 1f);
        }
        else
        {
            feedbackText.text = "Oops! Try again!";
        }
    }

    void NextStep()
    {
        currentQuestion++;
        if (currentQuestion < questions.Length)
            LoadQuestion();
        else
            Invoke(nameof(FinishMiniGame), 1f);
    }

    void FinishMiniGame()
    {
        gameActive = false;
        gameObject.SetActive(false);
        scenarioManager?.PuzzleCompleted(gameObject);
    }

    void GameOver()
    {
        gameActive = false;

        // disable interaction
        foreach (var button in answerButtons)
            button.interactable = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
}
