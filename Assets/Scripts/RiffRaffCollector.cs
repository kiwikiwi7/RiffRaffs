using System;
using System.Collections.Generic;
using UnityEngine;

public class RiffRaffCollector : MonoBehaviour
{
    public List<RiffRaffData> collectedRiffRaffs = new();
    public event Action OnCollectionChanged;

    public void Collect(RiffRaffData riffRaff)
    {
        if (!collectedRiffRaffs.Contains(riffRaff))
        {
            collectedRiffRaffs.Add(riffRaff);
            Debug.Log($"Collected {riffRaff.riffRaffName}! Total: {collectedRiffRaffs.Count}");

            // Show collection popup
            if (RiffRaffCollectUI.Instance != null)
            {
                RiffRaffCollectUI.Instance.Show(riffRaff);
            }
            else
            {
                Debug.LogWarning("No RiffRaffCollectUI instance found!");
            }

            OnCollectionChanged?.Invoke();
        }
    }
}
