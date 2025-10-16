using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject inventoryPanel;

    private RiffRaffCollector collector;
    private List<RiffRaffData> currentPartyList = new();

    private void Start()
    {
        collector = Object.FindFirstObjectByType<RiffRaffCollector>();
        if (collector != null)
        {
            collector.OnCollectionChanged += UpdateUI;
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("No RiffRaffCollector found in scene!");
        }
    }

    private void OnDestroy()
    {
        if (collector != null)
            collector.OnCollectionChanged -= UpdateUI;
    }

    private void Update()
    {
        // Toggle inventory visibility with I key
        if (Keyboard.current.iKey.wasPressedThisFrame)
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        bool active = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(active);

        if (active)
            UpdateUI();
    }

    private void UpdateUI()
    {
        // Clear existing slots
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Populate with current collection
        foreach (var riffRaff in collector.collectedRiffRaffs)
        {
            if (riffRaff == null) continue;

            var slot = Instantiate(slotPrefab, contentParent);
            var slotUI = slot.GetComponent<RiffRaffSlotUI>();

            if (slotUI != null)
                slotUI.Setup(riffRaff, this); // Pass this UI for toggling party members
            else
                Debug.LogError("Slot prefab missing RiffRaffSlotUI component!");
        }
    }

    public void TogglePartyMember(RiffRaffData riffRaff)
    {
        if (currentPartyList.Contains(riffRaff))
        {
            currentPartyList.Remove(riffRaff);
        }
        else if (currentPartyList.Count < 4)
        {
            currentPartyList.Add(riffRaff);
        }

        // Update followers behind the player
        if (PartyManager.Instance != null)
            PartyManager.Instance.SetParty(currentPartyList);

        // Refresh all slot button texts
        foreach (Transform child in contentParent)
        {
            var slotUI = child.GetComponent<RiffRaffSlotUI>();
            if (slotUI != null)
                slotUI.UpdateButtonLabel();
        }
    }

    public bool IsInParty(RiffRaffData riffRaff)
    {
        return currentPartyList.Contains(riffRaff);
    }
}
