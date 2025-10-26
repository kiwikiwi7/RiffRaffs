using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmNoteUI : MonoBehaviour
{
    private float speed;
    private RhythmLaneUI lane;
    private RectTransform rect;
    private float despawnY;
    private Image noteImage;
    private bool isHit = false;

    public RhythmLaneUI CurrentLane => lane;
    public RectTransform RectTransform => rect;

    public void Init(float moveSpeed, RhythmLaneUI owningLane, Vector2 startAnchoredPos, float despawnYValue)
    {
        rect = GetComponent<RectTransform>();
        noteImage = GetComponent<Image>();

        speed = moveSpeed;
        lane = owningLane;
        despawnY = despawnYValue;

        rect.SetParent(RhythmManagerUI.Instance.noteParent, false);
        rect.anchoredPosition = startAnchoredPos;
    }

    private void Update()
    {
        if (rect == null || isHit) return;

        // move down in UI space
        rect.anchoredPosition -= new Vector2(0f, speed * Time.deltaTime);

        // miss check
        if (rect.anchoredPosition.y < despawnY)
        {
            StartCoroutine(MissFeedback());
        }
    }

    public void Hit()
    {
        if (isHit) return;
        isHit = true;

        StartCoroutine(HitFeedback());
    }

    private IEnumerator HitFeedback()
    {
        float duration = 0.15f;
        float timer = 0f;

        Vector3 startScale = rect.localScale;
        Vector3 endScale = startScale * 1.5f;

        Color startColor = noteImage != null ? noteImage.color : Color.white;
        Color endColor = startColor;
        endColor.a = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            rect.localScale = Vector3.Lerp(startScale, endScale, t);

            if (noteImage != null)
                noteImage.color = Color.Lerp(startColor, endColor, t);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator MissFeedback()
    {
        isHit = true; // stop further movement
        float duration = 0.25f;
        float timer = 0f;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, -50f);

        Color startColor = noteImage != null ? noteImage.color : Color.white;
        Color missColor = Color.red;
        missColor.a = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            if (noteImage != null)
                noteImage.color = Color.Lerp(startColor, missColor, t);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
