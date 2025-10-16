using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform partyParent; // New: parent for the party slots (optional separate section)

    private RiffRaffCollector collector;
    private List<RiffRaffData> currentPartyList = new(); // New: stores your current 4 RiffRaffs

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

        if (partyParent != null)
        {
            foreach (Transform child in partyParent)
                Destroy(child.gameObject);
        }

        // Populate with current collection
        foreach (var riffRaff in collector.collectedRiffRaffs)
        {
            if (riffRaff == null) continue;

            Transform parent = currentPartyList.Contains(riffRaff) && partyParent != null
                ? partyParent
                : contentParent;

            var slot = Instantiate(slotPrefab, parent);
            var slotUI = slot.GetComponent<RiffRaffSlotUI>();

            if (slotUI != null)
                slotUI.Setup(riffRaff, this); // Pass reference for click callback
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

        UpdateUI();

        // Tell PartyManager to refresh the visual followers
        if (PartyManager.Instance != null)
            PartyManager.Instance.SetParty(currentPartyList);
    }
}
