using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField, TextArea] private string dialogue = "Hello!";

    public override void Interact(PlayerController player)
    {
        Debug.Log($"NPC says: {dialogue}");
        // Later: trigger dialogue UI here
    }
}