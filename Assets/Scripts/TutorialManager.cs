using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private TutorialUI tutorialUI; // handles text + hint display

    private TutorialPhase currentPhase = TutorialPhase.Intro;

    private bool hasMoved = false;
    private bool hasPickedUpRiffRaff = false;
    private bool hasOpenedInventory = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Subscribe to InputWatcher events
        if (InputWatcher.Instance != null)
        {
            InputWatcher.Instance.OnMove += HandlePlayerMove;
            InputWatcher.Instance.OnInventory += HandlePlayerInventory;
        }
        else
        {
            Debug.LogWarning("TutorialManager: No InputWatcher found in scene!");
        }

        StartCoroutine(TutorialRoutine());
    }

    private IEnumerator TutorialRoutine()
    {
        // === INITIAL SETUP ===
        player.enabled = false;
        blackScreen.SetActive(true);

        // === INTRO ===
        currentPhase = TutorialPhase.Intro;
        tutorialUI.ShowText("Welcome to Riffiya! You must be new here.");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("In this land, music is valued above all else.");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("For a time, there was no better way to play music than with the riffraffs!");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("Unfortunately, in the modern day, Riffraffs are a relic of the past.");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("But that’s where you come in!");
        yield return tutorialUI.WaitForNext();

        // === WORLD REVEAL ===
        currentPhase = TutorialPhase.WorldReveal;
        blackScreen.SetActive(false);
        tutorialUI.ShowText("This is Abeau, Guito, and Bitbam, my personal Riffraffs!");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("I want to pass them on to you.");
        yield return tutorialUI.WaitForNext();


        // === MOVEMENT PHASE ===
        currentPhase = TutorialPhase.Move;
        player.enabled = true;
        tutorialUI.ShowHint("Use WASD to move");
        yield return new WaitUntil(() => hasMoved);
        tutorialUI.HideHint();

        // === INTERACTION PHASE ===
        currentPhase = TutorialPhase.ChooseRiffRaff;
        tutorialUI.ShowHint("Press 'E' to pick up a RiffRaff");
        yield return new WaitUntil(() => hasPickedUpRiffRaff);
        tutorialUI.HideHint();

        // === INVENTORY PHASE ===
        currentPhase = TutorialPhase.Inventory;
        tutorialUI.ShowHint("Press 'I' to open your inventory");
        yield return new WaitUntil(() => hasOpenedInventory);
        tutorialUI.HideHint();

        // === FREE PLAY ===
        currentPhase = TutorialPhase.FreePlay;
        tutorialUI.ShowText("You are now free to explore the world and spread your music!");
        yield return tutorialUI.WaitForNext();
        tutorialUI.ShowText("When you feel your party is ready to perform, head to the stage and try your hand at a performance!");
        yield return tutorialUI.WaitForNext();

        CompleteTutorial();
    }

    // === EVENT HANDLERS ===
    private void HandlePlayerMove()
    {
            hasMoved = true;
    }

    public void OnPickedUpRiffRaff()
    {
        if (currentPhase == TutorialPhase.ChooseRiffRaff)
            hasPickedUpRiffRaff = true;
    }


    private void HandlePlayerInventory()
    {
        if (currentPhase == TutorialPhase.Inventory)
            hasOpenedInventory = true;
    }

    // === COMPLETION ===
    private void CompleteTutorial()
    {
        Debug.Log("Tutorial complete! Free play unlocked.");
        tutorialUI.HideAll();
        currentPhase = TutorialPhase.Completed;
        // Optional: save player progress here
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        if (InputWatcher.Instance != null)
        {
            InputWatcher.Instance.OnMove -= HandlePlayerMove;
            InputWatcher.Instance.OnInventory -= HandlePlayerInventory;
        }
    }
}

public enum TutorialPhase
{
    Intro,
    WorldReveal,
    Move,
    ChooseRiffRaff,
    Inventory,
    FreePlay,
    Completed
}
