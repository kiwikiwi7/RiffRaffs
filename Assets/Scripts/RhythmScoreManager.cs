using UnityEngine;
using TMPro;
using System;

public class RhythmScoreManager : MonoBehaviour
{
    public static RhythmScoreManager Instance { get; private set; }

    [Header("Judgment Popup")]
    [SerializeField] private GameObject judgmentPopupPrefab;
    [SerializeField] private RectTransform popupParent;

    [Header("Score Display")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;

    [Header("Score Settings")]
    [SerializeField] private int perfectScore = 100;
    [SerializeField] private int goodScore = 70;
    [SerializeField] private int missPenalty = 0;

    private int score = 0;
    private int combo = 0;
    private int maxCombo = 0;

    private int totalNotes = 0;
    private int successfulHits = 0;

    public event Action<int, int> OnScoreChanged; // (score, combo)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    // Called when a note is hit
    public void RegisterHit(float accuracy, Vector3 hitPosition)
    {
        totalNotes++;
        successfulHits++;

        int points;
        string judgment;
        Color color;

        if (accuracy < 10f)
        {
            points = perfectScore;
            judgment = "Perfect!";
            color = Color.yellow;
        }
        else if (accuracy < 25f)
        {
            points = goodScore;
            judgment = "Good!";
            color = Color.green;
        }
        else
        {
            points = goodScore / 2;
            judgment = "Okay!";
            color = Color.cyan;
        }

        score += points;
        combo++;
        if (combo > maxCombo) maxCombo = combo;

        SpawnPopup(judgment, color, hitPosition);
        UpdateUI();

        Debug.Log($"✅ {judgment} (+{points})  |  Combo {combo}");

        OnScoreChanged?.Invoke(score, combo);
    }

    // Called when a note is missed
    public void RegisterMiss(Vector3 hitPosition)
    {
        totalNotes++;
        combo = 0;
        score += missPenalty;

        SpawnPopup("Miss!", Color.red, hitPosition);
        UpdateUI();

        Debug.Log("❌ Miss!");

        OnScoreChanged?.Invoke(score, combo);
    }

    // Creates floating popup text
    private void SpawnPopup(string text, Color color, Vector3 position)
    {
        if (judgmentPopupPrefab == null || popupParent == null) return;

        var popup = Instantiate(judgmentPopupPrefab, popupParent);
        popup.transform.position = position;

        var popupUI = popup.GetComponent<JudgmentPopupUI>();
        if (popupUI != null)
            popupUI.Show(text, color);
    }

    // Updates score + combo UI
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (comboText != null)
            comboText.text = combo > 0 ? $"Combo: {combo}" : "";
    }

    // 🔹 Resets score state (used when performance starts)
    public void ResetScore()
    {
        score = 0;
        combo = 0;
        maxCombo = 0;
        totalNotes = 0;
        successfulHits = 0;

        UpdateUI();
        OnScoreChanged?.Invoke(score, combo);
    }
    public void ClearPopups()
    {
        if (popupParent == null) return;

        foreach (Transform child in popupParent)
        {
            Destroy(child.gameObject);
        }
    }

    // 🔹 Accessors for PerformanceManager
    public int GetScore() {
        float multiplier = RiffRaffMultiplierManager.Instance.GetPartyMultiplier();
        int finalPoints = Mathf.RoundToInt(score * multiplier);
        return finalPoints;
    }

        
    public int GetMaxCombo() => maxCombo;

    public float GetAccuracy()
    {
        if (totalNotes == 0) return 0f;
        return (successfulHits / (float)totalNotes) * 100f;
    }
}
