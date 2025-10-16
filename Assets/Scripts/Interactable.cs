using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [TextArea] public string promptText = "Press E to interact";

    // Override this in subclasses
    public virtual void Interact(PlayerController player)
    {
        Debug.Log($"{player.name} interacted with {gameObject.name}");
    }
}