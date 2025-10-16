using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RiffRaffSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject nameText;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonLabel; 

    private RiffRaffData riffRaff;
    private InventoryUI inventoryUI;

    public void Setup(RiffRaffData riffRaff, InventoryUI inventoryUI)
    {
        this.riffRaff = riffRaff;
        this.inventoryUI = inventoryUI;

        icon.GetComponent<Image>().sprite = riffRaff.sprite;
        nameText.GetComponent<TMP_Text>().text = riffRaff.riffRaffName;

        UpdateButtonLabel();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => inventoryUI.TogglePartyMember(riffRaff));
    }

    public void UpdateButtonLabel()
    {
        if (buttonLabel == null) return;

        bool inParty = inventoryUI.IsInParty(riffRaff);
        buttonLabel.text = inParty ? "Remove from Party" : "Add to Party";
        Debug.Log("Button text changed");
    }
}
