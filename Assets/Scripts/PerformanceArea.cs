using UnityEngine;

public class PerformanceArea : MonoBehaviour
{
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered performance area!");
            PerformanceManager.Instance.StartPerformance();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player left performance area!");
        }
    }
}