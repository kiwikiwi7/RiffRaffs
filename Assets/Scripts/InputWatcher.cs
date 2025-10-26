using UnityEngine;
using System;

public class InputWatcher : MonoBehaviour
{
    public static InputWatcher Instance { get; private set; }

    // 🔸 Events fired when player performs an action for the first time
    public event Action OnMove;
    public event Action OnInteract;
    public event Action OnInventory;

    // Internal flags so events only fire once
    private bool hasMoved = false;
    private bool hasOpenedInventory = false;

    private void Awake()
    {
        // Singleton pattern — ensures only one InputWatcher exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // ---- MOVEMENT ----
        // Fires when player first presses any movement key (WASD or arrows)
        if (!hasMoved)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (moveX != 0 || moveY != 0)
            {
                hasMoved = true;
                OnMove?.Invoke();
                Debug.Log("📢 InputWatcher: Player moved");
            }
        }

        /* ---- INTERACT (E) ----
        if (!hasInteracted && Input.GetKeyDown(KeyCode.E))
        {
            hasInteracted = true;
            OnInteract?.Invoke();
            Debug.Log("📢 InputWatcher: Player interacted (E)");
        }
        */

        // ---- INVENTORY (I) ----
        if (!hasOpenedInventory && Input.GetKeyDown(KeyCode.I))
        {
            hasOpenedInventory = true;
            OnInventory?.Invoke();
            Debug.Log("📢 InputWatcher: Player opened inventory (I)");
        }
    }

    // Optional: reset for replaying tutorial or reloading scene
    public void ResetWatcher()
    {
        hasMoved = false;
        hasOpenedInventory = false;
    }
}
