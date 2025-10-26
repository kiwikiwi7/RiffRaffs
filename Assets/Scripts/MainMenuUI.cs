using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Overworld"; // <-- main scene name

    [Header("Loading Screen")]
    [SerializeField] private CanvasGroup loadingScreen;  // a black screen with “Loading…” text
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        if (loadingScreen != null)
        {
            loadingScreen.alpha = 0f;
            loadingScreen.gameObject.SetActive(false);
        }
    }

    private void OnStartClicked()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(true);

            // Fade in
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
            loadingScreen.alpha = 1f;
        }

        // Async load the game scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            if (loadingText != null)
                loadingText.text = "Loading... " + Mathf.RoundToInt(asyncLoad.progress * 100f) + "%";
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        // Activate the scene
        asyncLoad.allowSceneActivation = true;
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
