using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiffRaffCollectUI : MonoBehaviour
{
    public static RiffRaffCollectUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        continueButton.onClick.AddListener(Hide);
    }

    public void Show(RiffRaffData riffRaff)
    {
        panel.SetActive(true);

        icon.sprite = riffRaff.sprite;
        nameText.text = riffRaff.riffRaffName;
        descriptionText.text = riffRaff.description;

        //Time.timeScale = 0f; // pause world
    }

    private void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
