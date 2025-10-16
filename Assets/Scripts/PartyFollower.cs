using UnityEngine;

public class PartyFollower : MonoBehaviour
{
    public Transform followTarget;
    public float followSpeed = 5f;
    public float followDistance = 1f;

    private Vector3 targetPosition;

    private void Update()
    {
        if (followTarget == null) return;

        targetPosition = followTarget.position - (followTarget.forward * followDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}
