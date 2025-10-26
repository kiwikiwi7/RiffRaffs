using UnityEngine;
using TMPro;
using System.Collections;

public class JudgmentPopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float floatDistance = 50f;
    [SerializeField] private float duration = 0.6f;

    public void Show(string text, Color color)
    {
        popupText.text = text;
        popupText.color = color;
        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        Color startColor = popupText.color;
        Color endColor = startColor;
        endColor.a = 0f;

        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            popupText.color = Color.Lerp(startColor, endColor, t);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
