using UnityEngine;

public class RhythmNoteUI : MonoBehaviour
{
    private float speed;
    private RhythmLaneUI lane;
    private RectTransform rect;
    private float despawnY;

    public RhythmLaneUI CurrentLane => lane;
    public RectTransform RectTransform => rect;

    // 👇 This is the correct Init() signature — 4 parameters
    public void Init(float moveSpeed, RhythmLaneUI owningLane, Vector2 startAnchoredPos, float despawnYValue)
    {
        rect = GetComponent<RectTransform>();
        speed = moveSpeed;
        lane = owningLane;
        despawnY = despawnYValue;

        // ensure the note is inside the same UI parent
        rect.SetParent(RhythmManagerUI.Instance.noteParent, false);

        // set its anchored position relative to the parent UI
        rect.anchoredPosition = startAnchoredPos;
    }

    private void Update()
    {
        if (rect == null) return;

        // move down in UI space
        rect.anchoredPosition -= new Vector2(0f, speed * Time.deltaTime);

        if (rect.anchoredPosition.y < despawnY)
            Destroy(gameObject);
    }

    public void Hit()
    {
        Debug.Log("Hit note in lane " + lane.name);
        Destroy(gameObject);
    }
}
