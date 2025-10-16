using System.Collections.Generic;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    [SerializeField] private Transform player; // player to follow
    [SerializeField] private GameObject riffRaffFollowerPrefab; // simple prefab with a SpriteRenderer

    private List<GameObject> spawnedFollowers = new();

    private void OnEnable()
    {
        // Subscribe to party changes
        PartyManager.Instance ??= Object.FindAnyObjectByType<PartyManager>();
        if (PartyManager.Instance != null)
        {
            // You can make this an event later, but for now we refresh manually
            RefreshParty();
        }
    }

    public void RefreshParty()
    {
        // Clear old followers
        foreach (var follower in spawnedFollowers)
        {
            if (follower != null)
                Destroy(follower);
        }
        spawnedFollowers.Clear();

        // Spawn new followers for each party member
        for (int i = 0; i < PartyManager.Instance.partyMembers.Count; i++)
        {
            RiffRaffData riffRaff = PartyManager.Instance.partyMembers[i];

            GameObject newFollower = Instantiate(riffRaffFollowerPrefab, player.position, Quaternion.identity);
            newFollower.name = riffRaff.riffRaffName + "_Follower";

            // Set sprite
            var sr = newFollower.GetComponent<SpriteRenderer>();
            if (sr != null && riffRaff.sprite != null)
            {
                sr.sprite = riffRaff.sprite;
            }

            // Add and configure PartyFollower
            var followerScript = newFollower.AddComponent<PartyFollower>();
            followerScript.followTarget = (i == 0)
                ? player
                : spawnedFollowers[i - 1].transform; // follow the previous one
            followerScript.followDistance = 5f;
            followerScript.followSpeed = 5f;

            spawnedFollowers.Add(newFollower);
        }
    }
}
