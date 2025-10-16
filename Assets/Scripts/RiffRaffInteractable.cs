using UnityEngine;

public class RiffRaffInteractable : Interactable
{
    [SerializeField] private RiffRaffData riffRaffData;

    public override void Interact(PlayerController player)
    {
        // Get collector on player
        if (player.TryGetComponent(out RiffRaffCollector collector))
        {
            collector.Collect(riffRaffData);
            Debug.Log($"{riffRaffData.riffRaffName} collected!");

            // Hide or destroy this world object
            Destroy(gameObject);
        }
    }
}