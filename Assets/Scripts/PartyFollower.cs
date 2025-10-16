using UnityEngine;

public class PartyFollower : MonoBehaviour
{
    public Transform followTarget;
    public float followDistance = 5f;
    public float followSpeed = 5f;

    private Vector3 targetOffset;

    private void Start()
    {
        // Tiny offset so followers aren’t stacked perfectly
        targetOffset = new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(-0.1f, 0.1f), 0);
    }

    private void Update()
    {
        if (followTarget == null) return;

        // Get direction from this follower to the target
        Vector3 toTarget = followTarget.position - transform.position;
        float distance = toTarget.magnitude;

        // If too far, move closer
        if (distance > followDistance)
        {
            Vector3 moveDir = toTarget.normalized;
            Vector3 targetPos = followTarget.position - moveDir * followDistance + targetOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}
