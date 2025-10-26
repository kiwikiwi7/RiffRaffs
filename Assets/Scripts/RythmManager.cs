using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RhythmManagerUI : MonoBehaviour
{
    public static RhythmManagerUI Instance { get; private set; }

    [SerializeField] private GameObject notePrefab;      // UI note prefab (with RectTransform)
    [SerializeField] public RectTransform noteParent;    // parent container
    [SerializeField] private List<RhythmLaneUI> lanes;   // each lane has a HitZone RectTransform
    [SerializeField] private float noteSpeed = 500f;
    [SerializeField] private float spawnY = 200f;
    [SerializeField] private float despawnY = -200f;
    [SerializeField] private float noteInterval = 0.5f;  // seconds between each spawn
    [SerializeField] private float songDuration = 20f;   // total length of the "song" in seconds

    private bool isPlaying = false;
    private readonly List<GameObject> activeNotes = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Called by PerformanceManager
    public void StartSong()
    {
        if (isPlaying) return;
        isPlaying = true;

        StartCoroutine(SongRoutine());
    }

    private IEnumerator SongRoutine()
    {
        Debug.Log("🎵 Song started!");
        StartCoroutine(SpawnNotesSpacedOut());
        yield return new WaitForSeconds(songDuration);
        EndSong();
    }

    private IEnumerator SpawnNotesSpacedOut()
    {
        float elapsed = 0f;

        while (elapsed < songDuration)
        {
            for (int i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                var noteObj = Instantiate(notePrefab, noteParent);
                var noteUI = noteObj.GetComponent<RhythmNoteUI>();

                float startX = lane.HitZone.position.x - 350f;
                Vector2 startAnchoredPos = new Vector2(startX, spawnY);

                noteUI.Init(noteSpeed, lane, startAnchoredPos, despawnY);
                activeNotes.Add(noteObj);

                //Debug.Log($"Spawned note in lane {lane.name} at {Time.time}");
                yield return new WaitForSeconds(noteInterval);
            }

            elapsed += noteInterval * lanes.Count;
        }
    }

    public void EndSong()
    {
        if (!isPlaying) return;
        isPlaying = false;

        Debug.Log("🎵 Song ended!");
        StopAllCoroutines();

        // 🔹 Clean up any active notes
        foreach (var note in activeNotes)
        {
            if (note != null)
                Destroy(note);
        }
        activeNotes.Clear();

        // 🔹 Also clear any judgment popups
        if (RhythmScoreManager.Instance != null)
            RhythmScoreManager.Instance.ClearPopups();

        // 🔹 End the performance (shows results, re-enables player, etc.)
        if (PerformanceManager.Instance != null)
            PerformanceManager.Instance.EndPerformance();
    }
}
