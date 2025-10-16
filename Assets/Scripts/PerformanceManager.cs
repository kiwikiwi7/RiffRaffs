using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager Instance { get; private set; }

    [SerializeField] private GameObject rhythmUI;
    [SerializeField] private GameObject player;

    private bool isPerforming = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartPerformance()
    {
        if (isPerforming) return;

        isPerforming = true;
        rhythmUI.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;

        Debug.Log("Performance started!");
        RhythmManagerUI.Instance.StartSong();
        // later: start song timing, note spawning, etc.
    }

    public void EndPerformance()
    {
        if (!isPerforming) return;

        isPerforming = false;
        rhythmUI.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;

        Debug.Log("Performance ended!");
    }
}
