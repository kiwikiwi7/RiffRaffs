using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup dialogueGroup;   // for fade-in/out of dialogue box
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueHintText;   // "Press Space to continue"
    [SerializeField] private TMP_Text hintText;           // for control hints ("Use WASD to move")

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.3f;

    private bool waitingForNext = false;

    private void Awake()
    {
        HideAll();
    }

    public void ShowText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInDialogue(text));
    }

    private IEnumerator FadeInDialogue(string text)
    {
        dialogueGroup.alpha = 0f;
        dialogueGroup.gameObject.SetActive(true);
        dialogueText.text = text;
        continueHintText.gameObject.SetActive(true);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            dialogueGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        dialogueGroup.alpha = 1f;
    }

    public IEnumerator WaitForNext()
    {
        waitingForNext = true;
        continueHintText.gameObject.SetActive(true);

        // 👇 Wait until the player releases the key or mouse button before listening again
        yield return new WaitUntil(() =>
            !Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0)
        );

        // Now wait for the *next* press
        yield return new WaitUntil(() =>
            Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
        );

        waitingForNext = false;
        continueHintText.gameObject.SetActive(false);
    }

    public void ShowHint(string hint)
    {
        hintText.gameObject.SetActive(true);
        hintText.text = hint;
    }

    public void HideHint()
    {
        hintText.gameObject.SetActive(false);
    }

    public void HideAll()
    {
        dialogueGroup.alpha = 0f;
        dialogueGroup.gameObject.SetActive(false);
        continueHintText.gameObject.SetActive(false);
        hintText.gameObject.SetActive(false);
    }
}
