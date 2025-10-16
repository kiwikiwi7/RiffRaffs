using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmLaneUI : MonoBehaviour
{
    [SerializeField] private Key keyToPress;
    [SerializeField] private RectTransform hitZone;
    [SerializeField] private float hitRange = 50f; // pixels

    public RectTransform HitZone => hitZone;

    private void Update()
    {
        if (Keyboard.current[keyToPress].wasPressedThisFrame)
            CheckHit();
    }

    private void CheckHit()
    {
        foreach (Transform child in RhythmManagerUI.Instance.noteParent)
        {
            var note = child.GetComponent<RhythmNoteUI>();
            if (note != null && note.CurrentLane == this)
            {
                float distance = Mathf.Abs(note.RectTransform.position.y - hitZone.position.y);
                if (distance <= hitRange)
                {
                    note.Hit();
                    return;
                }
            }
        }

        Debug.Log("Miss!");
    }
}
