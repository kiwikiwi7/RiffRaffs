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
    [SerializeField] private float noteInterval = 0.5f; // seconds between each spawn

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // call this to start the performance test
    public void StartSong()
    {
        StartCoroutine(SpawnNotesSpacedOut());
    }

    private IEnumerator SpawnNotesSpacedOut()
    {
        for (int i = 0; i < lanes.Count; i++)
        {
            var lane = lanes[i];

            var noteObj = Instantiate(notePrefab, noteParent);
            var noteUI = noteObj.GetComponent<RhythmNoteUI>();

            float startX = lane.HitZone.position.x - 350f;
            Vector2 startAnchoredPos = new Vector2(startX, spawnY);

            noteUI.Init(noteSpeed, lane, startAnchoredPos, despawnY);

            Debug.Log($"Spawned note in lane {lane.name} at time {Time.time}");
            yield return new WaitForSeconds(noteInterval); // ⏱ wait before next note
        }

        Debug.Log($"Spawned {lanes.Count} test notes (1 per lane)");
    }
}
