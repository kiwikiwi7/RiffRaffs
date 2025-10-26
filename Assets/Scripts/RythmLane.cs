using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class RhythmLaneUI : MonoBehaviour
{
    [SerializeField] private Key keyToPress;
    [SerializeField] private RectTransform hitZone;
    [SerializeField] private float hitRange = 50f; // pixels
    [SerializeField] private Image hitZoneImage; // assign the Image component of the hitZone
    [SerializeField] private float flashDuration = 0.1f;

    private Color defaultColor;

    public RectTransform HitZone => hitZone;

    private void Start()
    {
        if (hitZoneImage == null)
            hitZoneImage = hitZone.GetComponent<Image>();

        if (hitZoneImage != null)
            defaultColor = hitZoneImage.color;
    }

    private void Update()
    {
        if (Keyboard.current[keyToPress].wasPressedThisFrame)
            CheckHit();
    }

    private void CheckHit()
    {
        bool hitSomething = false;
        float closestDistance = float.MaxValue;

        foreach (Transform child in RhythmManagerUI.Instance.noteParent)
        {
            var note = child.GetComponent<RhythmNoteUI>();
            if (note != null && note.CurrentLane == this)
            {
                float distance = Mathf.Abs(note.RectTransform.position.y - hitZone.position.y);
                if (distance <= hitRange)
                {
                    note.Hit();
                    hitSomething = true;
                    closestDistance = distance;

                    RhythmScoreManager.Instance?.RegisterHit(distance, hitZone.position);
                    break;
                }
            }
        }

        if (!hitSomething)
            RhythmScoreManager.Instance?.RegisterMiss(hitZone.position);

        if (hitZoneImage != null)
            StartCoroutine(FlashColor(hitSomething ? Color.green : Color.red));
    }



    private IEnumerator FlashColor(Color flashColor)
    {
        hitZoneImage.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        hitZoneImage.color = defaultColor;
    }
}
