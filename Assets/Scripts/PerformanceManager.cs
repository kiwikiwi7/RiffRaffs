using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject rhythmUI;
    [SerializeField] private GameObject resultsUI;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalComboText;
    [SerializeField] private TMP_Text finalAccuracyText;
    [SerializeField] private TMP_Text riffRaffMultText;

    [Header("Performance Party Display")]
    [SerializeField] private Transform performancePartyParent; // 👈 Shown during performance
    [SerializeField] private GameObject partyMemberUIPrefab;      // prefab with Image + TMP_Text

    [SerializeField] private GameObject player;

    private readonly List<GameObject> performancePartyObjects = new();
    public List<GameObject> GetPerformancePartyObjects() => performancePartyObjects;

    public bool isPerforming = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (resultsUI != null)
            resultsUI.SetActive(false);
    }

    public void StartPerformance()
    {
        if (isPerforming) return;

        isPerforming = true;
        rhythmUI.SetActive(true);
        resultsUI.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;

        // ✅ Reset rhythm score before starting
        if (RhythmScoreManager.Instance != null)
            RhythmScoreManager.Instance.ResetScore();

        // ✅ Show current party in the performance panel
        PopulatePerformanceParty();
        StartPerformanceNoteSpawners();

        Debug.Log("Performance started!");
        PartyManager.Instance.SwitchToPerformanceMode(true);
        RhythmManagerUI.Instance.StartSong();

    }


    public void EndPerformance()
    {
        if (!isPerforming) return;

        isPerforming = false;
        rhythmUI.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;

        EndPerformanceNoteSpawners();

        PartyManager.Instance.SwitchToPerformanceMode(false);

        Debug.Log("Performance ended!");
        ShowResults();
    }

    private void ShowResults()
    {
        if (resultsUI == null) return;

        var scoreMgr = RhythmScoreManager.Instance;
        if (scoreMgr == null) return;

        int finalScore = scoreMgr.GetScore();
        int maxCombo = scoreMgr.GetMaxCombo();
        float accuracy = scoreMgr.GetAccuracy();
        float multiplier = RiffRaffMultiplierManager.Instance.GetPartyMultiplier();

        if (finalScoreText != null)
            finalScoreText.text = $"Score: {finalScore}";
        if (finalComboText != null)
            finalComboText.text = $"Max Combo: {maxCombo}";
        if (finalAccuracyText != null)
            finalAccuracyText.text = $"Accuracy: {accuracy:F1}%";
        if (riffRaffMultText != null)
            riffRaffMultText.text = $"Synergy Multiplier: x{multiplier}";

        resultsUI.SetActive(true);
    }

    private void PopulatePerformanceParty()
    {
        foreach (var obj in GetPerformancePartyObjects())
            Destroy(obj);
        performancePartyObjects.Clear();

        // Clear any previous entries
        foreach (Transform child in performancePartyParent)
            Destroy(child.gameObject);

        List<RiffRaffData> currentParty = PartyManager.Instance.partyMembers;

        foreach (var riff in currentParty)
        {
            var uiObj = Instantiate(partyMemberUIPrefab, performancePartyParent);
            var image = uiObj.GetComponentInChildren<Image>();
            var text = uiObj.GetComponentInChildren<TMP_Text>();

            if (image != null && riff.sprite != null)
                image.sprite = riff.sprite;
            if (text != null)
                text.text = riff.riffRaffName;
            performancePartyObjects.Add(uiObj);
        }        
        
    }

    private void StartPerformanceNoteSpawners()
    {
        foreach (var obj in GetPerformancePartyObjects())
        {
            var spawner = obj.GetComponent<MusicNoteSpawner>();
            if (spawner != null)
            {
                spawner.StartSpawning();
                Debug.Log("im spawning it im spawning it on " + obj.GetComponentInChildren<TMP_Text>().text);
            } 
        }

    }

    private void EndPerformanceNoteSpawners()
    {
        foreach (var obj in GetPerformancePartyObjects())
        {
            var spawner = obj.GetComponent<MusicNoteSpawner>();
            if (spawner != null)
            {
                spawner.StopSpawning();
            }
        }
    }

    public void ContinueAfterResults()
    {
        resultsUI.SetActive(false);
    }

}
