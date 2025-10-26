using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class MusicNoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private float interval = 1.5f;
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private Transform noteParent; // Can be UI panel or world space
    [SerializeField] private Color noteColor = Color.white;

    private Coroutine spawnRoutine;
    private bool isSpawning = false;

    // Keep track of currently active notes
    private readonly List<GameObject> activeNotes = new();

    public void StartSpawning()
    {
        if (isSpawning) return;
        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnNotes());
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;
        isSpawning = false;

        // Stop spawning new notes
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        // Destroy all currently active notes immediately
        foreach (var note in activeNotes)
        {
            Destroy(note);
        }
        activeNotes.Clear();
    }

    private IEnumerator SpawnNotes()
    {
        while (true)
        {
            if (noteParent != null)
            {
                var noteObj = Instantiate(notePrefab, noteParent);
                activeNotes.Add(noteObj);

                StartCoroutine(FloatAndFadeUI(noteObj));

                yield return new WaitForSeconds(interval);
            } else {
                var note = Instantiate(notePrefab, transform.position, Quaternion.identity);
                activeNotes.Add(note);

                var sr = note.GetComponent<SpriteRenderer>();
                if (sr != null) sr.color = noteColor;

                StartCoroutine(FloatAndFade(note));
                yield return new WaitForSeconds(interval);
            }
                  
        }
    }

    private IEnumerator FloatAndFade(GameObject note)
    {
        float elapsed = 0f;
        var sr = note.GetComponent<SpriteRenderer>();

        Color startColor = sr ? sr.color : Color.white;

        while (elapsed < lifetime)
        {
            if (note != null)
            {
                note.transform.position += Vector3.up * (riseSpeed * Time.deltaTime);
                if (sr != null)
                    sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - (elapsed / lifetime));
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (note != null)
        {
            Destroy(note);
            activeNotes.Remove(note);
        }
    }
    private IEnumerator FloatAndFadeUI(GameObject note)
    {
        float elapsed = 0f;
        var sr = note.GetComponent<Image>();

        Color startColor = sr ? sr.color : Color.white;

        while (elapsed < lifetime)
        {
            if (note != null)
            {
                note.transform.position += Vector3.up * 5 *(riseSpeed * Time.deltaTime);
                if (sr != null)
                    sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - (elapsed / lifetime));
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (note != null)
        {
            Destroy(note);
            activeNotes.Remove(note);
        }
    }
}
